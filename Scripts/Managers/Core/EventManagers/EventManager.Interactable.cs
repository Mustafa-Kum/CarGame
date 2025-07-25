
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Gate;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class InteractableEvents
        {
            public static UnityAction<InteractableData> Interact;
            
            public static UnityAction<GateInteractableData> GateInteract;
            
            public static UnityAction<BoostInteractableData> BoostInteract;

            public static UnityAction SlowMotionChaseEffect;
            
            public static UnityAction SpeedBoostInteract;
            
            public static UnityAction SpeedReduceInteract;
            
            public static UnityAction CopCrashableObstacleInteract;
        }
    }
}