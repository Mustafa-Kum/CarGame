using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class ObstacleInteractableEvents
        {
            public static UnityAction<float> ObstacleInteract;
        }
    }
}