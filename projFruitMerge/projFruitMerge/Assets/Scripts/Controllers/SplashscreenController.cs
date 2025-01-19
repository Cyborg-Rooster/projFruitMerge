using System;
using System.Collections;
using UnityEngine;

public class SplashscreenController : MonoBehaviour
{
    [SerializeField] GameObject Censor;
    [SerializeField] float time;
    [SerializeField] LanguageInitializer LanguageInitializer;
    [SerializeField] AdsInitializer AdsInitializer;

    AudioSource AudioSource;

    bool loaded;
    private void Awake()
    {
        StartCoroutine(WaitUntilInitialize());
    }

    IEnumerator WaitUntilInitialize()
    {
        int l = LanguageInitializer.GetLocalizationIndex();
        LanguageInitializer.Initialize();


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

        AdsInitializer.Initialize();
        yield return new WaitUntil(() => AdsInitializer.Initialized == true);

        loaded = true;
    }

    void Start()
    {

        AudioSource = GetComponent<AudioSource>();

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
        yield return new WaitUntil(() => loaded = true);
        SceneLoaderManager.LoadScene(1);
    }
}
