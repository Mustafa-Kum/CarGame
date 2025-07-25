using System;
using _Game.Scripts.General;
using Handler.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class GameStartHandler : MonoBehaviour
    {
        #region Unity Methods

        private void Awake()
        {
            EventManager.InGameEvents.GameStarted?.Invoke();
        }

        #endregion
        
    }
}
