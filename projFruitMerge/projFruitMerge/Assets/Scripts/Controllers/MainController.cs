using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] FadeController FadeController;
    [SerializeField] ParallaxController Dialog;
    [SerializeField] GameObject Version;
    [SerializeField] MainLanguageController LanguageController;
    [SerializeField] ToggleController SoundsToggle;
    [SerializeField] ToggleController MusicsToggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LanguageController.Translate(Player.Language);


        if (Player.Sounds == 0) SoundsToggle.Turn();
        if (Player.Musics == 0) MusicsToggle.Turn();

        UIManager.SetText(Version, "v"+Application.version);
        StartCoroutine(FadeController.FadeOut());


        Debug.Log("Sounds: " + Player.Sounds + ", Musics:" + Player.Musics);
    }

    public void GoToGame()
    {
        StartCoroutine(WaitUntilCloseAd());
    }

    public void Pause()
    {
        Dialog.Moving = true;
    }

    public void Unpause()
    {
        var rt = Dialog.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, -397);

        Dialog.Moving = false;
        SaveGame();
    }

    public void SetMusicsOn(ToggleController MusicsToggle)
    {
        Player.Musics = MusicsToggle.On ? 1 : 0;
    }

    public void SetSoundsOn(ToggleController SoundsToggle)
    {
        Player.Sounds = SoundsToggle.On ? 1 : 0;
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
        yield return FadeController.FadeIn();

        SceneLoaderManager.LoadScene(2);
    }
}
