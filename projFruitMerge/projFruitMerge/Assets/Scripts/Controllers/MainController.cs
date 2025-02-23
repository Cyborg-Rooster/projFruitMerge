using System;
using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] FadeController FadeController;
    [SerializeField] ParallaxController Form;
    [SerializeField] ParallaxController Dialog;

    [SerializeField] GameObject Options;
    [SerializeField] GameObject Ranking;

    [SerializeField] GameObject IDDialog;
    [SerializeField] GameObject OfflineDialog;
    [SerializeField] GameObject ReconnectDialog;
    [SerializeField] GameObject ReconnectDialogText;

    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject OptionButton;
    [SerializeField] GameObject RankingButton;

    [SerializeField] GameObject ID;
    [SerializeField] GameObject Version;
    [SerializeField] GameObject YourPosition;
    [SerializeField] GameObject BestScoreText;
    [SerializeField] RankingController RankingController;
    [SerializeField] MainLanguageController LanguageController;

    [SerializeField] ToggleController SoundsToggle;
    [SerializeField] ToggleController MusicsToggle;
    [SerializeField] MusicController MusicController;
    [SerializeField] AudioVolumeController SoundVolumeController;
    [SerializeField] AudioVolumeController MusicVolumeController;

    bool connected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LanguageController.Translate(Player.Language);

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

        UIManager.SetText(Version, "v" + Application.version);
        UIManager.SetText(BestScoreText, Player.BestScore);

        StartCoroutine(AskToReconnect());

        StartCoroutine(FadeController.FadeOut());
        StartCoroutine(MusicController.FadeOut());
    }

    IEnumerator AskToReconnect()
    {
        if (ServerManager.GetSucessfull != true || ServerManager.PostSucessfull != true)
        {
            UIManager.SetText(ReconnectDialogText, ReconnectMessagesInitializer.GetMessage(Player.Language));
            Dialog.Moving = true;
            OfflineDialog.SetActive(true);
            ReconnectDialog.SetActive(false);
            IDDialog.SetActive(false);

            UIManager.SetButtonEnable(PlayButton, false);
            UIManager.SetButtonEnable(OptionButton, false);
            UIManager.SetButtonEnable(RankingButton, false);

            yield return new WaitUntil(() => connected);
        }

        ReconnectDialog.SetActive(false);

        if (ServerManager.FirstTime)
        {
            ServerManager.FirstTime = false;
            UIManager.SetText(ID, Player.IDUser);

            OfflineDialog.SetActive(false);
            IDDialog.SetActive(true);
            Dialog.Moving = true;
        }
        else CloseDialog();

        RankingController.Refresh(LanguageController.GetScoreString(Player.Language));
        UIManager.SetText(YourPosition, ServerManager.Ranking.user_position);

        UIManager.SetButtonEnable(PlayButton, true);
        UIManager.SetButtonEnable(OptionButton, true);
        UIManager.SetButtonEnable(RankingButton, true);

        InvokeRepeating("CheckConnectionEveryTenSeconds", 10, 10);
    }

    IEnumerator TryToReconnect()
    {
        yield return new WaitForSeconds(.2f);

        OfflineDialog.SetActive(false);
        ReconnectDialog.SetActive(true);

        if (ServerManager.FirstTime)
        {
            yield return ServerManager.SendPostRequest();

            if (ServerManager.PostSucessfull == true)
            {
                Player.IDUser = ServerManager.OnlinePlayer.IDUser;

                SQLiteManager.RunQuery
                (
                    CommonQuery.Update
                    (
                        "PLAYER",
                        $"BEST_SCORE = {Player.BestScore}, " +
                        $"SOUNDS = {Player.Sounds}, " +
                        $"MUSICS = {Player.Musics}, " +
                        $"LANGUAGE = {Player.Language}, " +
                        $"IDUSER = '{Player.IDUser}', " +
                        $"APIKEY = '{Player.ApiKey}'",
                        "BEST_SCORE = BEST_SCORE"
                    )
                );
            }
        }
        else ServerManager.PostSucessfull = true;

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
            connected = false;
            CancelInvoke();
            StartCoroutine(AskToReconnect());
        }
    }

    private void OnApplicationQuit()
    {
        SQLiteManager.SetDatabaseActive(false);
    }

    public void Reconnect()
    {
        StartCoroutine(TryToReconnect());
    }

    public void GoToGame()
    {
        StartCoroutine(WaitUntilCloseAd());
    }

    public void Pause()
    {
        Ranking.SetActive(false);
        Options.SetActive(true);

        Form.Moving = true;
    }

    public void SetRanking()
    {
        Ranking.SetActive(true);
        Options.SetActive(false);

        Form.Moving = true;
    }

    public void Unpause()
    {
        var rt = Form.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -397);

        Form.Moving = false;
        SaveGame();
    }

    public void CloseDialog()
    {
        var rt = Dialog.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -337);

        Dialog.Moving = false;
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

        Debug.Log("Sounds: " + Player.Sounds + ", Musics:" + Player.Musics);
    }

    IEnumerator WaitUntilCloseAd()
    {
        FadeController.gameObject.SetActive(true);
        StartCoroutine(MusicController.FadeIn());
        yield return FadeController.FadeIn();

        SceneLoaderManager.LoadScene(2);
    }
}
