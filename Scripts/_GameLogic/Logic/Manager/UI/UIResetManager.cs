using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts._GameLogic.Logic.Manager.UI
{
    public class UIResetManager : MonoBehaviour
    {
        #region Public Variables

        public UnityEvent OnGameEndUIReset;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += HandleOnLevelEnd;
            EventManager.InGameEvents.LevelFail += HandleOnLevelEnd;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= HandleOnLevelEnd;
            EventManager.InGameEvents.LevelFail -= HandleOnLevelEnd;
        }

        #endregion

        #region Private Methods

        private void HandleOnLevelEnd()
        {
            OnGameEndUIReset?.Invoke();
        }

        #endregion
    }
}