using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    AudioSource AudioSource;

    [SerializeField] float MusicVolume;
    [SerializeField] float Duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public IEnumerator FadeOut()
    {
        if (Player.Musics == 1)
        {
            AudioSource = GetComponent<AudioSource>();

            while (AudioSource.volume < MusicVolume)
            {
                AudioSource.volume += Time.deltaTime / Duration * MusicVolume;
                yield return null;
            }

            AudioSource.volume = MusicVolume;
        }
    }

    public IEnumerator FadeIn()
    {
        if (Player.Musics == 1)
        {
            while (AudioSource.volume > 0)
            {
                AudioSource.volume -= Time.deltaTime / Duration * MusicVolume;
                yield return null;
            }

            AudioSource.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
