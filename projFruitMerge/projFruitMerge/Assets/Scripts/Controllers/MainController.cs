using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] FadeController FadeController;
    [SerializeField] ParallaxController Form;
    [SerializeField] ParallaxController Dialog;

    [SerializeField] GameObject Options;
    [SerializeField] GameObject Ranking;

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

        if (ServerManager.Online == true)
        {
            RankingController.CreateRanking(LanguageController.GetScoreString(Player.Language));
            UIManager.SetText(YourPosition, ServerManager.Ranking.user_position);

            if (ServerManager.FirstTime)
            {
                ServerManager.FirstTime = false;
                UIManager.SetText(ID, Player.IDUser);
                Dialog.Move();
            }
        }

        StartCoroutine(FadeController.FadeOut());
        StartCoroutine(MusicController.FadeOut());
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
        rt.anchoredPosition = new Vector2(0, -397);

        Form.Moving = false;
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

        Debug.Log(Player.Sounds);

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
