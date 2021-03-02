using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour/*, IUnityAdsListener*/
{
    /*#if UNITY_ANDROID
        private string _storeID = "3878587";
    #elif UNITY_IPHONE
        private string _storeID = "3878586";
    #else
        private string _storeID = "unexpected_platform";
    #endif
    
    private string _bannerAd = "bannerAd";
    private string _gameOverVideoAd = "gameOverRewardedAd";
    private string _newRingAd = "newRingAd";

    private bool _testMode = false;

    private void Start()
    {
        Advertisement.Initialize(_storeID, _testMode);
        Advertisement.AddListener (this);
        Advertisement.Banner.SetPosition (BannerPosition.BOTTOM_CENTER);
        StartCoroutine(ShowBannerAd());
        GameEvents.ONLevelComplete += ShowInterstitialAd;
    }
    private void ShowInterstitialAd() 
    {
        if ((PlayerPrefs.GetInt("CurrentLevel") + 1) > 3)
        {
            Invoke(nameof(InterstitialAdDelay), 1f);
        }
    }
    private void InterstitialAdDelay()
    {
        if (Advertisement.IsReady()) 
        {
            Advertisement.Show();
        }
    }
    private IEnumerator ShowBannerAd () {
        while (!Advertisement.isInitialized) {
            yield return BetterWaitForSeconds.Wait(0.5f);
        }
        Advertisement.Banner.Show (_bannerAd);
    }

    public void ShowGameOverAd() 
    {
        if (Advertisement.IsReady(_gameOverVideoAd)) 
        {
            Advertisement.Show(_gameOverVideoAd);
        }
    }

    public void ShowNewRingAd() 
    {
        if (Advertisement.IsReady(_newRingAd)) 
        {
            Advertisement.Show(_newRingAd);
        }
    }

    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) 
    {
        GameManager.Instance.UnpauseGame();
        
        if (placementId == _gameOverVideoAd)
        {
            switch (showResult)
            {
                case  ShowResult.Finished:
                    GameEvents.LevelContinue();
                    break;
                case ShowResult.Skipped:
                    GameManager.Instance.RestartGame();
                    break;
                case ShowResult.Failed:
                    GameManager.Instance.RestartGame();
                    break;
            }
        }
        else if (placementId == _newRingAd)
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    GameEvents.NewRingUnlock();
                    break;
                case ShowResult.Skipped:
                    GameManager.Instance.NextLevel();
                    break;
                case ShowResult.Failed:
                    GameManager.Instance.NextLevel();
                    break;
            }
        }
    }

    public void OnUnityAdsReady (string placementId) 
    {
    }

    public void OnUnityAdsDidError (string message) 
    {
        GameManager.Instance.NextLevel();
    }

    public void OnUnityAdsDidStart (string placementId)
    {
        GameManager.Instance.PauseGame();
    } 

    public void OnDestroy() 
    {
        GameEvents.ONLevelComplete -= ShowInterstitialAd;
        Advertisement.RemoveListener(this);
    }*/
}
