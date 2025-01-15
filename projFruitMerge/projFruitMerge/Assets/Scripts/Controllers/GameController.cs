using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject PointText;
    [SerializeField] GameObject BestScoreText;
    [SerializeField] GameObject NewText;
    [SerializeField] LanguageController LanguageController;

    [SerializeField] ParallaxController PointUI;
    [SerializeField] ParallaxController Dialog;
    [SerializeField] GameObject GameOver;
    [SerializeField] GameObject Options;
    [SerializeField] ToggleController SoundsToggle;
    [SerializeField] ToggleController MusicsToggle;

    [Header("Game Over Position")]
    [SerializeField] Vector2 StartPosition;
    [SerializeField] LayerMask LayerMask;
    [SerializeField] float Distance;
    [SerializeField] int Time;

    RaycastManager RaycastManager;

    public int Points;

    public bool OnGame = true;

    private void Awake()
    {
        int l = LanguageController.GetLocalizationIndex();
        LanguageController.Initialize();

        Player.BestScore = 0;
        Player.Sounds = 1;
        Player.Musics = 1;
        Player.Language = l;

        if (SQLiteManager.Initialize())
        {
            object[] data = SQLiteManager.ReturnValues(CommonQuery.Select("*", "PLAYER"));

            Player.BestScore = Convert.ToInt32(data[0]);
            Player.Sounds = Convert.ToInt32(data[1]);
            Player.Musics = Convert.ToInt32(data[2]);
            Player.Language = Convert.ToInt32(data[3]);
        }

        LanguageController.Translate(Player.Language);
    }

    private void Start()
    {
        if(Player.Sounds == 0) SoundsToggle.Turn();
        if (Player.Musics == 0) MusicsToggle.Turn();

        RaycastManager = new RaycastManager();
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

        if (Player.BestScore < Points)
        { 
            Player.BestScore = Points; 
            NewText.SetActive(true);
        }

        TextManager.SetText(BestScoreText, Player.BestScore);

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
        Debug.Log(Player.Musics);
    }

    public void SetSoundsOn(ToggleController SoundsToggle)
    {
        Player.Sounds = SoundsToggle.On ? 1 : 0;
        Debug.Log(Player.Sounds);
    }

    public void ChangeLanguage(bool ascending)
    {
        if (ascending)
        {
            Player.Language++;
            if (Player.Language > LanguageController.GetLanguageLenght()) Player.Language = 0;
        }
        else
        {
            Player.Language--;
            if (Player.Language < 0) Player.Language = LanguageController.GetLanguageLenght();
        }

        LanguageController.Translate(Player.Language);
    }

    public void AddPoints(int points)
    {
        Points += points;
        TextManager.SetText(PointText, Points);
    }

    public void Restart()
    {
        SceneLoaderManager.LoadScene(0);
    }
}
