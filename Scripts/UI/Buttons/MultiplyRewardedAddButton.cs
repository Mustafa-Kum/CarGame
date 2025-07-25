using System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using FortuneWheel;
using UnityEngine;

namespace _Game.Scripts.UI.Buttons
{
    public class MultiplyRewardedAddButton : ButtonBase
    {
        [SerializeField] private MultiplyRewardedAdData multiplyRewardedAdData;
        [SerializeField] private GameObject buttonToOpen;
        [SerializeField] private float secondsToWait = 3f;
        private CoroutineService _coroutineService;

        public int multiplierCount = 2;
        
        protected override void OnClicked()
        {
            EventManager.AdEvents.ShowRewarded?.Invoke(OnRewardedAddSuccessful);
        }

        private void OnEnable()
        {
            _coroutineService = new CoroutineService(this);

            WaitForSecondsToWaitAndActivateButtonToOpen();
        }

        private void WaitForSecondsToWaitAndActivateButtonToOpen()
        {
            _coroutineService.StartDelayedRoutine(ActivateButtonToOpen, secondsToWait);
            buttonToOpen.SetActive(false);
        }

        private void ActivateButtonToOpen()
        {
            buttonToOpen.SetActive(true);
        }
        
        private void OnRewardedAddSuccessful()
        {
            EventManager.CurrencySystem.RewardedAd?.Invoke(new RewardedAdData
            {
                collectableType = multiplyRewardedAdData.CollectableType,
                rewardAmount = multiplyRewardedAdData.RewardAmount * multiplierCount
            });


            //TODO : CLAIM
            Invoke("NextLevelMethod", 3.0f);
        }

        //TODO: CLAIM BUTTON
        private void NextLevelMethod()
        {
            buttonToOpen.GetComponent<NextLevelButton>().HandleClick();
        }
    }

    [Serializable]
    public struct MultiplyRewardedAdData
    {
        public int RewardAmount;
        public int MultiplyCount;
        public CollectableType CollectableType;
    }
}