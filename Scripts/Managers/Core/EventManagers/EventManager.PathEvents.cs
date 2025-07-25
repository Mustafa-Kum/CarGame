using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class PathEvents
        {
            public static UnityAction<int> PathChanged; 
        }   
    }
}