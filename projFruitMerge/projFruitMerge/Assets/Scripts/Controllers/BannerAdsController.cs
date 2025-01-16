using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

class BannerAdsController : MonoBehaviour
{
    [SerializeField] string AndroidAdBannerId = "Banner_Android";
    [SerializeField] string AndroidAdInterstitialId = "Interstitial_Android";

    string AdBannerID = null;

    bool AdBannerLoaded = false;

    void Start()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        AdBannerID = iOSAdBannerId;
#elif UNITY_ANDROID
        AdBannerID = AndroidAdBannerId;
#endif

        // Set the banner position:
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public IEnumerator WaitUntilBannerLoad()
    {
        LoadBanner();
        yield return new WaitUntil(() => AdBannerLoaded == true);
        ShowBannerAd();
    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(AdBannerID, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        AdBannerLoaded = true;
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            //clickCallback = OnBannerClicked,
            //hideCallback = OnBannerHidden,
            //showCallback = OnBannerShown
        };

        Advertisement.Banner.Show(AdBannerID, options);
    }


}
