using _Game.Scripts._GameLogic.Logic;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Interactables
{
    public class SplineObjectObstacleInteract : MonoBehaviour, IInteractableAction
    {
        [SerializeField] private float cutSpeedDivider = 1f;
        [SerializeField] private VehicleSpeedBoostValues vehicleSpeedBoostValues;
        
        public CollectableData CollectableData;
        
        public void InteractableAction()
        {
            EventManager.ObstacleInteractableEvents.ObstacleInteract?.Invoke(cutSpeedDivider);
            
            if (vehicleSpeedBoostValues.IsSpeedBoostActive)
                EventManager.CollectableEvents.Collect?.Invoke(CollectableData);
        }
    }
}