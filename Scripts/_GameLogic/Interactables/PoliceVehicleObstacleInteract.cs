using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Interactables
{
    public class PoliceVehicleObstacleInteract : MonoBehaviour , IInteractableAction
    {
        public void InteractableAction()
        {
            Debug.Log("Police Vehicle Obstacle Interacted");
            EventManager.InteractableEvents.CopCrashableObstacleInteract?.Invoke();
        }
    }
}