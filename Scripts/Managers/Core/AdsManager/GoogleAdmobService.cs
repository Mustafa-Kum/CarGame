using System;
using _Game.Scripts.Helper.Extensions.System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class GoogleAdmobService : IAdService
    {
        private RewardedAd rewardedAd;
        private InterstitialAd interstitialAd;
        private BannerView bannerView;
        private RewardedInterstitialAd _rewardedInterstitialAd;     
        

        //private const string InterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        private const string InterstitialRewardedAdUnitId = "ca-app-pub-4317683447749730/9265504863";
        private const string RewardedAdUnitId = "ca-app-pub-4317683447749730/5326259852";
        private const string BannerAdUnitId = "ca-app-pub-4317683447749730/1387014842";


        public void Initialize()
        {
            MobileAds.Initialize(initStatus =>
            {
                TDebug.Log("Google Mobile Ads initialized.");
                LoadRewardedAd();
                LoadInterstitialAd();
                LoadBannerAd();
                LoadRewardedInterstitialAd();
            });
        }

        public void Uninitialize()
        {
            rewardedAd?.Destroy();
            interstitialAd?.Destroy();
            bannerView?.Destroy();
            _rewardedInterstitialAd?.Destroy();
        }

        public void ShowRewardedAd(Action onAdSuccessful)
        {
            const string rewardMsg =
                "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    onAdSuccessful?.Invoke();
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }

        public void ShowInterstitialAd()
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                interstitialAd.Show();
                
                RegisterReloadHandler(interstitialAd);
            }
            else
            {
                TDebug.LogWarning("Interstitial ad not loaded.");
                LoadInterstitialAd();
            }
        }
        
        public void ShowRewardedInterstitialAd(Action onAdSuccessful)
        {
            const string rewardMsg =
                "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

            if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
            {
                _rewardedInterstitialAd.Show((Reward reward) =>
                {
                    onAdSuccessful?.Invoke();
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
                
                RegisterReloadHandler(_rewardedInterstitialAd);
            }
        }

        public void ShowBannerAd()
        {
            if (bannerView != null)
            {
                bannerView.Show();
            }
            else
            {
                TDebug.LogWarning("Banner ad not loaded.");
                LoadBannerAd();
            }
        }
        
        private void RegisterReloadHandler(InterstitialAd interstitialAd)
        {
            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += ( ) =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
            
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += error =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
        }
        
        private void RegisterReloadHandler(RewardedInterstitialAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded interstitial ad failed to open " +
                               "full screen content with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedInterstitialAd();
            };
        }
        
        private void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(RewardedAdUnitId, adRequest,
                (ad, error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                });
        }
        
        private void LoadInterstitialAd()
        {
            /*if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(InterstitialAdUnitId, adRequest,
                (ad, error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    interstitialAd = ad;
                });*/
        }
        
        public void LoadRewardedInterstitialAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedInterstitialAd != null)
            {
                _rewardedInterstitialAd.Destroy();
                _rewardedInterstitialAd = null;
            }

            Debug.Log("Loading the rewarded interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            // send the request to load the ad.
            RewardedInterstitialAd.Load(InterstitialRewardedAdUnitId, adRequest,
                (ad, error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedInterstitialAd = ad;
                });
        }

        private void LoadBannerAd()
        {
            // create an instance of a banner view first.
            if(bannerView == null)
            {
                CreateBannerView();
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            bannerView.LoadAd(adRequest);
        }

        private void CreateBannerView()
        {
            bannerView = new BannerView(BannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        }
    }
}
