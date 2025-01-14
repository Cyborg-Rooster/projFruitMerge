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

    [Header("Game Over Position")]
    [SerializeField] Vector2 StartPosition;
    [SerializeField] LayerMask LayerMask;
    [SerializeField] float Distance;
    [SerializeField] int Time;

    RaycastManager RaycastManager;
    Player Player;

    public int Points;

    public bool OnGame = true;

    private void Awake()
    {
        LanguageController.Initialize();
        LanguageController.Translate(LanguageController.GetLocalizationIndex());

        SQLiteManager.Initialize();
        object[] data = SQLiteManager.ReturnValues(CommonQuery.Select("*", "PLAYER"));

        Player = new Player()
        {
            BestScore = Convert.ToInt32(data[0]),
            Sounds = Convert.ToInt32(data[1]),
            Musics = Convert.ToInt32(data[2]),
            Language = Convert.ToInt32(data[3])
        };
    }

    private void Start()
    {
        RaycastManager = new RaycastManager();
    }

    private void Update()
    {
        if(RaycastManager.IsColliding(Distance, Distance, StartPosition, LayerMask) && OnGame)
        {
            EndGame();
        }
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
        StartCoroutine(WaitUntilNextFrame());
    }

    IEnumerator WaitUntilNextFrame()
    {
        yield return new WaitForEndOfFrame();
        OnGame = true;
    }
}
