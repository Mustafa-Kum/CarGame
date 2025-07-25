using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.UI.Buttons;
using FortuneWheel;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Collectables
{
    public class CollectableManager : MonoBehaviour
    {
        [SerializeField]
        private CurrencyValuesSO collectableValuesSO;

        private void OnEnable()
        {
            EventManager.CollectableEvents.Collect += HandleCollectEvent;
            EventManager.CurrencySystem.TryToBuy += HandleTryToBuyEvent;
            EventManager.CurrencySystem.RewardedAd += AddValueOfRewardedAdEvent;
        }

        private void OnDisable()
        {
            EventManager.CollectableEvents.Collect -= HandleCollectEvent;
            EventManager.CurrencySystem.TryToBuy -= HandleTryToBuyEvent;
            EventManager.CurrencySystem.RewardedAd -= AddValueOfRewardedAdEvent;
        }
        
        private void HandleTryToBuyEvent(int cost, CollectableType collectableType, UnityAction<bool> callback)
        {
            if (collectableValuesSO.GetValue(collectableType) >= cost)
            {
                collectableValuesSO.SpendValue(CollectableType.Coin, cost);
                callback?.Invoke(true);
                EventManager.CurrencySystem.CollectableSpent?.Invoke(collectableType);
            }
            else
            {
                callback?.Invoke(false);
            }
        }

        private void AddValueOfRewardedAdEvent(RewardedAdData rewardedAdData)
        {
            collectableValuesSO.AddValue(rewardedAdData.collectableType, rewardedAdData.rewardAmount);
            EventManager.CurrencySystem.CollectableSpent?.Invoke(rewardedAdData.collectableType);
        }
        
        private void HandleCollectEvent(CollectableData collectableData)
        {
            collectableValuesSO.AddValue(collectableData.Type, collectableData.ScoreAmount);
        }
    }
}
