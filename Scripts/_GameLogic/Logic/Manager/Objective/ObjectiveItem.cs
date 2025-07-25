using System;

namespace _Game.Scripts._GameLogic.Logic.Manager.Objective
{
    [Serializable]
    public class ObjectiveItem
    {
        public GridObjectType Type;
        public int RequiredCount;
        public ObjectiveItem(GridObjectType type, int requiredCount)
        {
            Type = type;
            RequiredCount = requiredCount;
        }




    }
}