using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Managers.Core;

namespace _Game.Scripts._GameLogic.Logic.Manager.Objective
{
    [Serializable]
    public class LevelProgressionItems
    {
        #region PUBLIC VARIABLES

         public  List<ObjectiveItem> Objectives;
        
        #endregion

        public LevelProgressionItems(List<ObjectiveItem> levelProgressionItems)
        {
            Objectives = new List<ObjectiveItem>(levelProgressionItems);
        }
        
        public List<ObjectiveItem> GetObjectives()
        {
            return new List<ObjectiveItem>(Objectives);
        }
  

        #region PUBLIC METHODS

        public void ReduceObjective(GridObjectType type)
        {
            var item = Objectives.Find(obj => obj.Type == type);
            if (item == null) return;
            
            item.RequiredCount--;
            EventManager.ObjectiveEvents.ObjectiveUpdated?.Invoke(type, item.RequiredCount);
            if (item.RequiredCount <= 0)
            {
                Objectives.Remove(item);
            }
        }
        
        public bool IsObjectivesCompleted() => Objectives.All(obj => obj.RequiredCount <= 0);

        #endregion
        
    }
}