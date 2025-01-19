using System;
using System.Collections;
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

        if (Player.Sounds == 0) SoundsToggle.Turn();
        if (Player.Musics == 0) MusicsToggle.Turn();

        RaycastManager = new RaycastManager();

        StartCoroutine(FadeController.FadeOut());
    }

    private void Update()
    {
        if(RaycastManager.IsColliding(Distance, Distance, StartPosition, LayerMask) && OnGame)
        {
            EndGame();
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

    private void EndGame()
    {
        OnGame = false;

        Options.SetActive(false);
        GameOver.SetActive(true);

        Dialog.Moving = true;
        PointUI.Moving = true;
        NextUI.Moving = true;
        OptionsUI.Moving = true;

        UIManager.SetButtonEnable(OptionsUI.gameObject, false);

        if (Player.BestScore < Points)
        { 
            Player.BestScore = Points; 
            NewText.SetActive(true);
        }

        UIManager.SetText(BestScoreText, Player.BestScore);

        SaveGame();
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

    public void AddPoints(int points)
    {
        Points += points;
        UIManager.SetText(PointText, Points);
    }

    public void Restart()
    {
        StartCoroutine(WaitUntilCloseAd());
    }

    IEnumerator WaitUntilCloseAd()
    {
        FadeController.gameObject.SetActive(true);
        yield return FadeController.FadeIn();

        if (AdsController.IntersticialLoaded)
        {
            AdsController.ShowInterstitialAd();
            yield return new WaitUntil(() => AdsController.InterstitialClosed == true);
        }

        SceneLoaderManager.LoadScene(0);
    }

}
