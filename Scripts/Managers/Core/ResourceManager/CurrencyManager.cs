using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts.Managers.Core.StoreManager;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using FortuneWheel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core.ResourceManager
{
    public class CurrencyManager : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private CurrencyValuesSO currencyValuesSo;
        [SerializeField] private ProductCatalogSO productCatalog;

        #endregion

        #region PRIVATE VARIABLES

        private IAPCurrencyStore _iapCurrencyStore;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            _iapCurrencyStore = new IAPCurrencyStore(currencyValuesSo, productCatalog);
            _iapCurrencyStore.InitializeProducts();
        }

        private void OnEnable()
        {
            EventManager.CollectableEvents.Collect += HandleCollectEvent;
            EventManager.Resource.TryToBuy += HandleTryToBuyEvent;
            EventManager.Resource.RewardedAd += AddValueOfRewardedAdEvent;
            EventManager.IAPEvents.PurchaseSuccess += HandlePurchaseSuccessEvent;
        }

        private void OnDisable()
        {
            EventManager.CollectableEvents.Collect -= HandleCollectEvent;
            EventManager.Resource.TryToBuy -= HandleTryToBuyEvent;
            EventManager.Resource.RewardedAd -= AddValueOfRewardedAdEvent;
            EventManager.IAPEvents.PurchaseSuccess -= HandlePurchaseSuccessEvent;
        }

        #endregion

        #region PRIVATE METHODS
        
        private void HandlePurchaseSuccessEvent(PurchaseEventArgs arg0)
        {
            _iapCurrencyStore.ProcessPurchase(arg0.purchasedProduct.definition.id);
        }

        private void HandleTryToBuyEvent(int cost, CollectableType currencyType, UnityAction<bool> callback)
        {
            if (currencyValuesSo.GetValue(currencyType) >= cost)
            {
                currencyValuesSo.SpendValue(CollectableType.Coin, cost);
                callback?.Invoke(true);
                EventManager.Resource.CollectableSpent?.Invoke(currencyType);
            }
            else
            {
                callback?.Invoke(false);
            }
        }

        private void AddValueOfRewardedAdEvent(RewardedAdData rewardedAdData)
        {
            currencyValuesSo.AddValue(rewardedAdData.collectableType, rewardedAdData.rewardAmount);
            EventManager.Resource.CollectableSpent?.Invoke(rewardedAdData.collectableType);
        }

        private void HandleCollectEvent(CollectableData collectableData)
        {
            currencyValuesSo.AddValue(collectableData.Type, collectableData.ScoreAmount);
        }

        #endregion
    }
}