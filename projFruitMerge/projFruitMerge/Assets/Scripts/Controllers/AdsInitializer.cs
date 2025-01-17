using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine;

public class AdsInitializer : MonoBehaviour
{
    public bool Initialized;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        MobileAds.Initialize(HandleInitializationComplete);
    }

    void HandleInitializationComplete(InitializationStatus initStatus)
    {
        Debug.Log("AdMob foi inicializado com sucesso!");
        Initialized = true;
    }

}
