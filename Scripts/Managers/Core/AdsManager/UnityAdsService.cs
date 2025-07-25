using System;
using _Game.Scripts.Helper.Extensions.System;

namespace _Game.Scripts.Managers.AdsManager
{
    sealed class UnityAdsService : IAdService
    {
        private Action _onAdSuccessful;
        
        public void ShowRewardedAd(Action onAdSuccessful)
        {
            TDebug.LogWarning("Unity Ads Service: ShowRewardedAd(Action onAdSuccessful) is not implemented. Debug mode is on. ");
            
            _onAdSuccessful = onAdSuccessful;
            
            #if UNITY_EDITOR
            _onAdSuccessful?.Invoke();
            #endif
        }

        public void ShowInterstitialAd()
        {
            // Implement Unity Ads logic for showing interstitial ads
        }

        public void GiveReward()
        {
            // Implement Unity Ads logic for giving rewards
            _onAdSuccessful?.Invoke();
        }
    }

    public interface IAdService
    {
        void ShowRewardedAd(Action onAdSuccessful);
        void ShowInterstitialAd();
        void GiveReward();
    }
}