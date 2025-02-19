using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    [SerializeField] private GameObject bestScoreText;
    [SerializeField] private GameObject actualPositionText;
    [SerializeField] private GameObject PlayGameText;

    [SerializeField] private GameObject IdTitle;
    [SerializeField] private GameObject Explanation;
    [SerializeField] private GameObject IdText;
    [SerializeField] private GameObject Understood;

    [SerializeField] private GameObject ConnectionTitle;
    [SerializeField] private GameObject Reconnect;
    [SerializeField] private GameObject Reconnecting;

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
        UIManager.SetText(PlayGameText, gameText.PlayMainText);
        UIManager.SetText(OptionsMainText, gameText.OptionsMainText);
        UIManager.SetText(bestScoreText, gameText.BestScoreText);
        UIManager.SetText(actualPositionText, gameText.ActualPositionText);
        UIManager.SetText(OptionsMainText, gameText.OptionsMainText);
        UIManager.SetText(IdTitle, gameText.IdTitle);
        UIManager.SetText(Explanation, gameText.Explanation);
        UIManager.SetText(IdText, gameText.IdText);
        UIManager.SetText(Understood, gameText.Understood);
        UIManager.SetText(ConnectionTitle, gameText.Offline);
        UIManager.SetText(Reconnect, gameText.Reconnect);
        UIManager.SetText(Reconnecting, gameText.Reconnecting);
    }

    public string GetScoreString(int index)
    {
        GameText gameText = LanguageInitializer.bruteValues[index.ToString()];

        return gameText.BestScoreText;
    }
}
