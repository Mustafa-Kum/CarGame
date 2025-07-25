using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class UIEvents
        {
            #region UI

            public static UnityAction OnSettingsButtonActivated;
            
            public static UnityAction OnSettingsButtonDeactivated;

            
            public static UnityAction<int> OnMoveCounterChanged;
            
            public static UnityAction OnMenuTabsChanged;

            public static UnityAction<bool> PurchaseRestoredResult;

            #endregion
        }
    }
}