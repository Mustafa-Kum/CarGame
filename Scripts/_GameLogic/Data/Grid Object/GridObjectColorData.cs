using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Grid_Object
{
    [CreateAssetMenu(fileName = "GridObjectColorData", menuName = "PuzzleGame/GridObjectColorData", order = 0)]
    public class GridObjectColorData : SerializedScriptableObject
    {
        #region Public Fields

        [OdinSerialize]
        public Dictionary<GridObjectType, Vector3> GridObjectRotationDictionary;
        
        //Key: Level Index, Value: Rotation Reduction
        [OdinSerialize]
        public Dictionary<int, int> RotationReductionByLevelIndex;
        
        public List<GridObjectType> GridObjectRotationOrder;
        
        public int levelIndex;
        
        public LevelList_SO levelListSo;

        #endregion

        #region Public Methods

        public Vector3 GetNextRotation(int currentRotationIndex)
        {
            levelIndex = levelListSo.GetActualLevel();
            int reductionCount = RotationReductionByLevelIndex.GetValueOrDefault(levelIndex, 0);
            int availableRotations = Mathf.Max(0, GridObjectRotationOrder.Count - reductionCount);
            if (availableRotations == 0)
            {
                return default(Vector3);
            }

            int adjustedIndex = currentRotationIndex % availableRotations;
            GridObjectType gridObjectType = GridObjectRotationOrder[adjustedIndex];
            if (GridObjectRotationDictionary.TryGetValue(gridObjectType, out Vector3 nextRotation))
            {
                return nextRotation;
            }

            return default(Vector3);
        }
        
        public int GetRotationReduction()
        {
            levelIndex = levelListSo.GetActualLevel();
            if (levelIndex >= levelListSo.AllLevels.Count) 
            {
                return 0;
            }
            return RotationReductionByLevelIndex.GetValueOrDefault(levelIndex);
        }
        #endregion
    }
}
