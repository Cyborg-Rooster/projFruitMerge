using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject PointText;
    [SerializeField] GameObject BestScoreText;
    [SerializeField] GameObject NewText;
    [SerializeField] FadeController FadeController;

    [Space()]
    [SerializeField] GameLanguageController LanguageController;
    [SerializeField] RankingController RankingController;
    [SerializeField] GameObject YourPosition;

    [Space()]
    [SerializeField] ParallaxController PointUI;
    [SerializeField] ParallaxController Form;
    [SerializeField] ParallaxController NextUI;
    [SerializeField] ParallaxController OptionsUI;
    [SerializeField] ParallaxController Dialog;
    [SerializeField] GameObject OfflineDialog;
    [SerializeField] GameObject ReconnectDialog;
    [SerializeField] GameObject ReconnectDialogText;

    [Space()]
    [SerializeField] GameObject GameOver;
    [SerializeField] GameObject Options;

    [Space()]
    [SerializeField] ToggleController SoundsToggle;
    [SerializeField] ToggleController MusicsToggle;
    [SerializeField] AudioVolumeController SoundVolumeController;
    [SerializeField] AudioVolumeController MusicVolumeController;

    [Header("Ads")]
    [SerializeField] AdsController AdsController;

    [Header("Game Over Position")]
    [SerializeField] Vector2 StartPosition;
    [SerializeField] LayerMask LayerMask;
    [SerializeField] float Distance;
    [SerializeField] int Time;

    RaycastManager RaycastManager;

    public int Points;

    public bool OnGame = true;
    bool connected;


    private void Start()
    {
        LanguageController.Translate(Player.Language);

        AdsController.LoadBanner();
        AdsController.LoadInterstitialAd();

        if (Player.Sounds == 0)
        {
            SoundVolumeController.SetVolume(0f);
            SoundsToggle.Turn();
        }
        else SoundVolumeController.SetVolume(0.3f);

        if (Player.Musics == 0)
        {
            MusicVolumeController.SetVolume(0f);
            MusicsToggle.Turn();
        }
        else MusicVolumeController.SetVolume(0.3f);

        RaycastManager = new RaycastManager();

        if (ServerManager.GetSucessfull == true && ServerManager.PostSucessfull == true)
        {
            RankingController.Refresh(LanguageController.GetScoreString(Player.Language));
            UIManager.SetText(YourPosition, ServerManager.Ranking.user_position);
        }

        StartCoroutine(FadeController.FadeOut());
        InvokeRepeating("CheckConnectionEveryTenSeconds", 10, 10);
    }

    IEnumerator AskToReconnect()
    {
        if (ServerManager.GetSucessfull != true || ServerManager.PostSucessfull != true)
        {
            Dialog.Moving = true;
            OfflineDialog.SetActive(true);
            ReconnectDialog.SetActive(false);

            UIManager.SetButtonEnable(OptionsUI.gameObject, false);

            OnGame = false;

            yield return new WaitUntil(() => connected);
        }

        ReconnectDialog.SetActive(false);
        CloseDialog();

        RankingController.Refresh(LanguageController.GetScoreString(Player.Language));
        UIManager.SetText(YourPosition, ServerManager.Ranking.user_position);

        OnGame = true;

        UIManager.SetButtonEnable(OptionsUI.gameObject, true);

        InvokeRepeating("CheckConnectionEveryTenSeconds", 10, 10);
    }

    IEnumerator TryToReconnect()
    {
        yield return new WaitForSeconds(.2f);

        OfflineDialog.SetActive(false);
        ReconnectDialog.SetActive(true);

        ServerManager.PostSucessfull = true;

        yield return ServerManager.SendGetRequest();
        yield return new WaitForSeconds(0.5f);

        if (ServerManager.GetSucessfull != true || ServerManager.PostSucessfull != true)
        {
            OfflineDialog.SetActive(true);
            ReconnectDialog.SetActive(false);
        }
        else connected = true;
    }

    private void CheckConnectionEveryTenSeconds()
    {
        StartCoroutine(CheckConnection());
    }

    private IEnumerator CheckConnection()
    {
        yield return ServerManager.CheckConnection();

        if (ServerManager.GetSucessfull != true || ServerManager.PostSucessfull != true)
        {
            UIManager.SetText(ReconnectDialogText, ReconnectMessagesInitializer.GetMessage(Player.Language));
            connected = false;
            CancelInvoke();
            StartCoroutine(AskToReconnect());
        }
    }

    public void Reconnect()
    {
        StartCoroutine(TryToReconnect());
    }

    public void CloseDialog()
    {
        var rt = Dialog.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -337);

        Dialog.Moving = false;
    }

    private void Update()
    {
        if(RaycastManager.IsColliding(Distance, Distance, StartPosition, LayerMask) && OnGame)
        {
            StartCoroutine(EndGame());
        }
    }

    private void SaveGame()
    {
        SQLiteManager.RunQuery
        (
            CommonQuery.Update
            (
                "PLAYER",
                $"BEST_SCORE = {Player.BestScore}, " +
                $"SOUNDS = {Player.Sounds}, " +
                $"MUSICS = {Player.Musics}, " +
                $"LANGUAGE = {Player.Language}",
                "BEST_SCORE = BEST_SCORE"
            )
        );
    }

    private IEnumerator EndGame()
    {
        UIManager.SetButtonEnable(OptionsUI.gameObject, false);

        if (Player.BestScore < Points)
        { 
            Player.BestScore = Points; 
            NewText.SetActive(true);

            if (ServerManager.GetSucessfull == true && ServerManager.PostSucessfull == true)
            {
                ServerManager.Ranking.ResortRanking(Player.BestScore);

                yield return WaitUntilGetPosition();
                RankingController.Refresh(LanguageController.GetScoreString(Player.Language));
            }
        }

        UIManager.SetText(BestScoreText, Player.BestScore);
        OnGame = false;

        Options.SetActive(false);
        GameOver.SetActive(true);

        Form.Moving = true;
        PointUI.Moving = true;
        NextUI.Moving = true;
        OptionsUI.Moving = true;

        SaveGame();
    }

    IEnumerator WaitUntilGetPosition()
    {
        yield return ServerManager.SendPutRequest();
        UIManager.SetText(YourPosition, ServerManager.Ranking.user_position);
    }

    IEnumerator WaitUntilNextFrame()
    {
        yield return new WaitForEndOfFrame();
        OnGame = true;
    }

    public void Pause()
    {
        OnGame = false;

        Options.SetActive(true);
        GameOver.SetActive(false);

        Form.Moving = true;
    }

    public void Unpause()
    {
        var rt = Form.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2 (0, -397);

        Form.Moving = false;
        SaveGame();
        StartCoroutine(WaitUntilNextFrame());
    }

    public void SetMusicsOn(ToggleController MusicsToggle)
    {
        Player.Musics = MusicsToggle.On ? 1 : 0;

        if (Player.Musics == 0) MusicVolumeController.SetVolume(0f);
        else MusicVolumeController.SetVolume(0.3f);
    }

    public void SetSoundsOn(ToggleController SoundsToggle)
    {
        Player.Sounds = SoundsToggle.On ? 1 : 0;

        if (Player.Sounds == 0) SoundVolumeController.SetVolume(0f);
        else SoundVolumeController.SetVolume(0.3f);
    }

    public void ChangeLanguage(bool ascending)
    {
        if (ascending)
        {
            Player.Language++;
            if (Player.Language > LanguageInitializer.GetLanguageLenght()) Player.Language = 0;
        }
        else
        {
            Player.Language--;
            if (Player.Language < 0) Player.Language = LanguageInitializer.GetLanguageLenght();
        }

        LanguageController.Translate(Player.Language);
    }

    public void AddPoints(int points)
    {
        Points += points;
        UIManager.SetText(PointText, Points);
    }

    public void Restart()
    {
        StartCoroutine(WaitUntilCloseAd(2));
    }

    public void GoToMain()
    {
        StartCoroutine(WaitUntilCloseAd(1));
    }

    IEnumerator WaitUntilCloseAd(int scene)
    {
        FadeController.gameObject.SetActive(true);
        yield return FadeController.FadeIn();

        if (AdsController.IntersticialLoaded && scene == 2)
        {
            AdsController.ShowInterstitialAd();
            yield return new WaitUntil(() => AdsController.InterstitialClosed == true);
        }

        SceneLoaderManager.LoadScene(scene);
    }

    private void OnApplicationQuit()
    {
        SQLiteManager.SetDatabaseActive(false);
    }

}
