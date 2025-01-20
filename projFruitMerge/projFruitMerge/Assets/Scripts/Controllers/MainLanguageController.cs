using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MainLanguageController : MonoBehaviour
{
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject soundsText;
    [SerializeField] private GameObject musicsText;
    [SerializeField] private GameObject languageText;
    [SerializeField] private GameObject respectiveLanguage;
    [SerializeField] private GameObject creditsText;
    [SerializeField] private GameObject developerText;
    [SerializeField] private GameObject artistText;
    [SerializeField] private GameObject audioText;
    [SerializeField] private GameObject distribuitionText;
    [SerializeField] private GameObject PlayMainText;
    [SerializeField] private GameObject OptionsMainText;


    public void Translate(int index)
    {
        GameText gameText = LanguageInitializer.bruteValues[index.ToString()];

        UIManager.SetText(titleText, gameText.TitleText);
        UIManager.SetText(soundsText, gameText.SoundsText);
        UIManager.SetText(musicsText, gameText.MusicsText);
        UIManager.SetText(languageText, gameText.LanguageText);
        UIManager.SetText(respectiveLanguage, gameText.RespectiveLanguage);
        UIManager.SetText(creditsText, gameText.CreditsText);
        UIManager.SetText(developerText, gameText.DeveloperText);
        UIManager.SetText(artistText, gameText.ArtistText);
        UIManager.SetText(audioText, gameText.AudioText);
        UIManager.SetText(distribuitionText, gameText.DistribuitionText);
        UIManager.SetText(PlayMainText, gameText.PlayMainText);
        UIManager.SetText(OptionsMainText, gameText.OptionsMainText);
    }


}
