using _Game.Scripts.General;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Gate
{
    public class EndMetaStarter : MonoBehaviour, IInteractableAction
    {
        #region Public Methods

        public void InteractableAction()
        {
            EventManager.InGameEvents.EndMetaStart?.Invoke();
            EventManager.InGameEvents.AfterLevelSuccess?.Invoke();
        }

        #endregion
    }
}