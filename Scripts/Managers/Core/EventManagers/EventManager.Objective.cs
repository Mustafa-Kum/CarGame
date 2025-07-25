using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Manager.Objective;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class ObjectiveEvents
        {
            public static UnityAction<GridObjectType, int> ObjectiveUpdated;
            
            public static UnityAction<List<ObjectiveItem>> LevelProgressionItemsUpdated;
        }
    }
}