using System;
using System.Collections;
using UnityEngine;

public class SplashscreenController : MonoBehaviour
{
    [SerializeField] GameObject Censor;
    [SerializeField] float time;

    [SerializeField] LanguageInitializer LanguageInitializer;
    [SerializeField] AdsInitializer AdsInitializer;
    [SerializeField] ReconnectMessagesInitializer ReconnectMessagesInitializer;

    [SerializeField] AudioVolumeController SoundVolumeController;
    [SerializeField] Animator Logo;

    [SerializeField] AudioClip Plim;

    AudioSource AudioSource;

    public bool loaded;

    IEnumerator WaitUntilInitialize()
    {
        int l = LanguageInitializer.GetLocalizationIndex();
        LanguageInitializer.Initialize();
        ReconnectMessagesInitializer.Initialize();

        Player.BestScore = 0;
        Player.Sounds = 1;
        Player.Musics = 1;
        Player.Language = l;
        Player.ApiKey = System.Guid.NewGuid().ToString();

        if (SQLiteManager.Initialize())
        {
            object[] data = SQLiteManager.ReturnValues(CommonQuery.Select("*", "PLAYER"));

            Player.BestScore = Convert.ToInt32(data[0]);
            Player.Sounds = Convert.ToInt32(data[1]);
            Player.Musics = Convert.ToInt32(data[2]);
            Player.Language = Convert.ToInt32(data[3]);
            Player.IDUser = data[4].ToString();
            Player.ApiKey = data[5].ToString();

            ServerManager.PostSucessfull = true;
        }
        else
        {
            ServerManager.FirstTime = true;

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

        StartCoroutine(ServerManager.SendGetRequest());

        bool audioOn = Player.Sounds == 1 || Player.Musics == 1;

        if (audioOn == true) SoundVolumeController.SetVolume(0.3f);
        else SoundVolumeController.SetVolume(0f);

        AdsInitializer.Initialize();
        yield return ServerManager.WaitUntilRankingDownloaded();
        yield return new WaitUntil(() => AdsInitializer.Initialized == true);

        loaded = true;
    }

    void Start()
    {

        AudioSource = GetComponent<AudioSource>();


        Logo.speed = 0;
        StartCoroutine(WaitUntilInitialize());

        InvokeRepeating("RemoveCensor", time, time);
        StartCoroutine(WaitUntilLoad());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveCensor()
    {
        Vector3 p = new Vector3
        (
            Censor.transform.position.x, 
            Censor.transform.position.y - 0.02f, 
            Censor.transform.position.z
        );

        Censor.transform.position = p;
    }

    IEnumerator WaitUntilLoad()
    {
        AudioSource.Play();
        yield return new WaitForSeconds(3f);

        AudioSource.clip = Plim;
        AudioSource.Play();
        Logo.speed = 1;

        yield return new WaitForSeconds(1f);

        Logo.gameObject.SetActive(false);

        yield return new WaitUntil(() => loaded = true);

        SceneLoaderManager.LoadScene(1);
    }
}
