#if UNITY_EDITOR
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Visual;
using _Game.Scripts._GameLogic.Logic.Grid;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.EditorFunctions
{
    public class EditorMatrixPointSelector
    {
        #region PRIVATE VARIABLES
        
        private readonly GridTile[,] _matrix;
        private readonly Vector2Int _startGridPosition;
        private readonly Vector2Int _endGridPosition;
        private readonly ParticleContainer _particleContainer;

        #endregion

        #region CONSTRUCTORS

        public EditorMatrixPointSelector(GridTile[,] matrix, Vector2Int startGridPosition, Vector2Int endGridPosition, ParticleContainer particleContainer)
        {
            _matrix = matrix;
            _startGridPosition = startGridPosition;
            _endGridPosition = endGridPosition;
            _particleContainer = particleContainer;
        }

        #endregion

        #region PUBLIC METHODS

        public void PerformTileIndicating()
        {
            List<GridTile> tiles = new List<GridTile>();

            if (IsPositionWithinBounds(_startGridPosition) && IsPositionWithinBounds(_endGridPosition))
            {
                tiles.Add(_matrix[_startGridPosition.x, _startGridPosition.y]);
                tiles.Add(_matrix[_endGridPosition.x, _endGridPosition.y]);

                var i = 0;
                
                foreach (var tile in tiles)
                {
                    PerformProduceIndicatorOnTiles(tile, i);
                    i++;
                }
            }
            else
            {
                Debug.LogError("Start or end positions are out of the matrix bounds.");
            }
        }

        #endregion

        #region PRIVATE METHODS

        private bool IsPositionWithinBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < _matrix.GetLength(0) &&
                   position.y >= 0 && position.y < _matrix.GetLength(1);
        }
        private void PerformProduceIndicatorOnTiles(GridTile tile, int index)
        {
            var tileTransform = tile.transform;
            Vector3 spawnPosition = tileTransform.position + new Vector3(0, 1.036f, 0);

            GameObject particleSystem = _particleContainer.GetParticle(index);
            
            GameObject indicatorInstance = PrefabUtility.InstantiatePrefab(particleSystem) as GameObject;
            
            //tileTransform.RemoveComponentInChildren<GridCubeOnPathAction>();

            if (indicatorInstance != null)
            {
                Transform transform;
                (transform = indicatorInstance.transform).SetParent(tileTransform.transform);
                transform.position = spawnPosition;
            }
            else
            {
                Debug.LogError("Failed to instantiate the indicator prefab.");
            }
        }
        

        #endregion
    }
}
#endif

