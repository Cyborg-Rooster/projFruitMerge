using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
class LanguageController : MonoBehaviour
{
    [SerializeField] private GameObject nextText;
    [SerializeField] private GameObject settingsText;
    [SerializeField] private GameObject bestScoreText;
    [SerializeField] private GameObject actualPositionText;
    [SerializeField] private GameObject rankingText;
    [SerializeField] private GameObject playGameText;
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
    [SerializeField] private GameObject backToMainText;

    [SerializeField] TextAsset Json;
    Dictionary<string, GameText> bruteValues;

    public void Initialize()
    {
        string jsonContent = Json.text;

        bruteValues = JsonConvert.DeserializeObject<Dictionary<string, GameText>>(jsonContent);
    }

    public void Translate(int index)
    {
        GameText gameText = bruteValues[index.ToString()];

        TextManager.SetText(nextText, gameText.NextText);
        TextManager.SetText(settingsText, gameText.SettingsText);
        TextManager.SetText(bestScoreText, gameText.BestScoreText);
        TextManager.SetText(actualPositionText, gameText.ActualPositionText);
        TextManager.SetText(rankingText, gameText.RankingText);
        TextManager.SetText(playGameText, gameText.PlayGameText);
        TextManager.SetText(titleText, gameText.TitleText);
        TextManager.SetText(soundsText, gameText.SoundsText);
        TextManager.SetText(musicsText, gameText.MusicsText);
        TextManager.SetText(languageText, gameText.LanguageText);
        TextManager.SetText(respectiveLanguage, gameText.RespectiveLanguage);
        TextManager.SetText(creditsText, gameText.CreditsText);
        TextManager.SetText(developerText, gameText.DeveloperText);
        TextManager.SetText(artistText, gameText.ArtistText);
        TextManager.SetText(audioText, gameText.AudioText);
        TextManager.SetText(distribuitionText, gameText.DistribuitionText);
        TextManager.SetText(backToMainText, gameText.BackToMainText);

    }

    public int GetLocalizationIndex()
    {
        CultureInfo cultureInfo = CultureInfo.CurrentCulture;
        if (cultureInfo.Name.Equals("pt-BR", System.StringComparison.OrdinalIgnoreCase)) return 0;
        else return 1;
    }
}
