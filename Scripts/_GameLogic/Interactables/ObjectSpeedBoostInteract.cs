using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Interactables
{
    public class ObjectSpeedBoostInteract : MonoBehaviour, IInteractableAction
    {
        public void InteractableAction()
        {
            EventManager.InteractableEvents.SpeedBoostInteract.Invoke();
        }
    }
}