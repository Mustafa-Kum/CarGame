using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class GridEvents
        {
            #region Grid

            public static UnityAction<Vector2Int> OnGridConfigCreated;
            
            public static UnityAction<GridObject> OnGridObjectClicked;
            
            public static UnityAction OnGridObjectDescend;
            
            public static UnityAction OnGridObjectAscend;
            
            public static UnityAction OnPathObjectSpawned;
            
            public static UnityAction<List<GridTile>> OnPathFound;
            
            public static UnityAction<List<GridTile>> OnExcludePathFound;
            
            public static UnityAction<List<GridTile>> OnGridObjectMatched;
            
            public static UnityAction<GridObjectType, Vector3> GridObjectMatchedType;
            
            #endregion
        }
    }
}