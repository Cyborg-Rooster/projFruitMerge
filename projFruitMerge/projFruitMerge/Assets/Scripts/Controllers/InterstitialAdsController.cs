using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAdsController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string AndroidAdUnitId = "Interstitial_Android";
    [SerializeField] string iOsAdUnitId = "Interstitial_iOS";
    [SerializeField] GameController GameController;

    string AdInterstitialId;
    public bool AdInterstitialLoaded;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        AdInterstitialId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOsAdUnitId
            : AndroidAdUnitId;
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + AdInterstitialId);
        Advertisement.Load(AdInterstitialId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + AdInterstitialId);
        Advertisement.Show(AdInterstitialId, this);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        AdInterstitialLoaded = true;
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        GameController.RealRestart();
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) 
    {

    }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        GameController.RealRestart();
    }
}