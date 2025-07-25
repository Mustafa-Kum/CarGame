using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Grid
{
    [CreateAssetMenu(fileName = "GridContainer", menuName = "PuzzleGame/GridContainer", order = 0)]
    public class GridTileContainer : ScriptableObject
    {
        #region Public Variables
        
        public GameObject gridPrefab;

        #endregion

        #region Public Methods

        public GameObject GetGridPrefab()
        {
            return gridPrefab;
        }

        #endregion
    }
}
