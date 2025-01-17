using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AdsController : MonoBehaviour
{
    [SerializeField] string AdBannerAndroid;
    [SerializeField] string AdBannerIos;

    [SerializeField] string AdIntersticialAndroid;
    [SerializeField] string AdIntersticialIOS;

    BannerView Banner;
    InterstitialAd Interstitial;

    public bool BannerLoaded;
    public bool IntersticialLoaded;
    public bool InterstitialClosed;
    public void LoadBanner()
    {
        if (Banner == null) CreateBanner();

        var adRequest = new AdRequest();

        Debug.Log("Carregando banner");
        Banner.LoadAd(adRequest);
    }

    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (Interstitial != null)
        {
            Interstitial.Destroy();
            Interstitial = null;
        }

        Debug.Log("Carregando interstitial");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(AdIntersticialAndroid, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial falhou ao carregar: " + error);
                    return;
                }
                else
                {
                    Debug.Log("Intersticial carregado.");
                    IntersticialLoaded = true;
                    Interstitial = ad;

                    Interstitial.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;
                }
            });
    }

    private void OnAdFullScreenContentClosed()
    {
        InterstitialClosed = true;
        Debug.Log("Intersticial fechado.");
    }

    public void DestroyBanner()
    {
        Debug.Log("Destruindo banner.");
        Banner.Destroy();
        Banner = null;
    }

    public void ShowInterstitialAd()
    {
        if (Interstitial != null && Interstitial.CanShowAd())
        {
            Debug.Log("Mostrando intersticial.");
            Interstitial.Show();
        }
        else Debug.LogError("Interstitial não está pronto.");
    }

    private void CreateBanner()
    {
#if UNITY_ANDROID
        Banner = new BannerView(AdBannerAndroid, AdSize.Banner, AdPosition.Bottom);
#elif UNITY_IPHONE
        Banner = new BannerView(AdBannerAndroid, AdSize.Banner, AdPosition.Bottom);
#endif

        Banner.OnBannerAdLoaded += OnBannerAdLoaded;
        Banner.OnBannerAdLoadFailed += OnBannerAdLoadFailed;
    }

    private void OnBannerAdLoadFailed(LoadAdError obj)
    {
        Debug.LogError("Banner não foi carregado: " + obj);
    }

    private void OnBannerAdLoaded()
    {
        Debug.Log("Banner carregado");
        BannerLoaded = true;
    }
}
