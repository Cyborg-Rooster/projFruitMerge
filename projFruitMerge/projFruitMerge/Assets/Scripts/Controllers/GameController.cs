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
    [SerializeField] ParallaxController Dialog;
    [SerializeField] ParallaxController NextUI;
    [SerializeField] ParallaxController OptionsUI;

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

        RankingController.CreateRanking(LanguageController.GetScoreString(Player.Language));
        UIManager.SetText(YourPosition, ServerManager.Ranking.user_position);

        StartCoroutine(FadeController.FadeOut());
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

            ServerManager.Ranking.ResortRanking(Player.BestScore);

            yield return WaitUntilGetPosition();
            RankingController.Refresh(LanguageController.GetScoreString(Player.Language));
        }

        UIManager.SetText(BestScoreText, Player.BestScore);
        OnGame = false;

        Options.SetActive(false);
        GameOver.SetActive(true);

        Dialog.Moving = true;
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

        Dialog.Moving = true;
    }

    public void Unpause()
    {
        var rt = Dialog.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2 (0, -397);

        Dialog.Moving = false;
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

}
