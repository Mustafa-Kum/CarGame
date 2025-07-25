using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Helper.Extensions.System;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Grid
{
    [CreateAssetMenu(fileName = "GridObjectContainer", menuName = "PuzzleGame/GridObjectContainer", order = 0)]
    public class GridObjectContainer : SerializedScriptableObject
    {
        #region Public Variables

        public Dictionary<GridObjectType, GridObject> gridObjectDictionary;

        #endregion
        
        #region Public Methods
        
        public GridObject GetRandomGridObject()
        {
            int randomIndex = Random.Range(0, gridObjectDictionary.Count);
            

            return gridObjectDictionary.ElementAt(randomIndex).Value;
        }
        
        public GridObject GetRandomColredGridObject()
        {
            //None,ObstacleMatched,Not Included look GridObjectType enum
            
            List<GridObjectType> gridObjectTypes = new List<GridObjectType>
            {
                GridObjectType.Red,
                GridObjectType.Blue,
                GridObjectType.Green,
                GridObjectType.Yellow,
                
            };
            
            int randomIndex = Random.Range(0, gridObjectTypes.Count);
            
            return GetGridObjectByType(gridObjectTypes[randomIndex]);
    
        }
        
        public GridObject GetGridObjectByType(GridObjectType gridObjectType)
        {
            if (gridObjectDictionary.TryGetValue(gridObjectType, out GridObject gridObjectPrefab))
            {
                return gridObjectPrefab;
            }
            else
            {
                TDebug.LogError($"Grid object type {gridObjectType} not found in dictionary.");
                return null;
            }
        }

        
        #endregion
        public void InitializeGridObjectContainer(GameObject parentObject)
        {
            // Add Lean foreach prefabs
            foreach (var gridObject in gridObjectDictionary)
            {
                LeanPool.Links.Add(gridObject.Value.gameObject, parentObject.AddComponent<LeanGameObjectPool>());
            }
           
        }
        public GameObject GetBombGridObject()
        {
            return gridObjectDictionary[GridObjectType.Bomb].gameObject;
        }
    }
}