using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using FortuneWheel;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class Resource
        {
            #region Currency

            public static UnityAction<int,CollectableType,UnityAction<bool>> TryToBuy;
            
            public static UnityAction<CollectableType> CollectableSpent;
            
            public static UnityAction<RewardedAdData> RewardedAd;
            
            public static UnityAction<CollectableType> CurrencyChanged;

            #endregion

            #region Health

            public static UnityAction HealthChanged;
            
            public static UnityAction InfiniteHealthSetted;
            
            #endregion

            #region Booster

            public static UnityAction BoosterValueChanged;            

            #endregion
        }
    }
}