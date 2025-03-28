﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
class GameLanguageController : MonoBehaviour
{

    [SerializeField] private GameObject nextText;
    [SerializeField] private GameObject settingsText;
    [SerializeField] private GameObject NewText;
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
    [SerializeField] private GameObject ConnectionTitle;
    [SerializeField] private GameObject Reconnect;
    [SerializeField] private GameObject Reconnecting;

    public void Translate(int index)
    {
        GameText gameText = LanguageInitializer.bruteValues[index.ToString()];

        UIManager.SetText(nextText, gameText.NextText);
        UIManager.SetText(settingsText, gameText.SettingsText);
        UIManager.SetText(NewText, gameText.NewText);
        UIManager.SetText(bestScoreText, gameText.BestScoreText);
        UIManager.SetText(actualPositionText, gameText.ActualPositionText);
        UIManager.SetText(rankingText, gameText.RankingText);
        UIManager.SetText(playGameText, gameText.PlayGameText);
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
        UIManager.SetText(backToMainText, gameText.BackToMainText);
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
