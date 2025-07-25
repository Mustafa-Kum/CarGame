using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class PotentialMatchSearcher
    {
        private GridTile[,] _matrix;
        private Vector2Int _startPoint;
        private Vector2Int _endPoint;
      

        public PotentialMatchSearcher(GridTile[,] matrix, Vector2Int startPoint, Vector2Int endPoint)
        {
            _matrix = matrix;
            _startPoint = startPoint;
            _endPoint = endPoint;
        }
        
        private bool IsValidPath(ref GridTile[,] matrix, ref Vector2Int startPoint, ref Vector2Int endPoint, IHeuristicCalculator heuristicCalculator)
        {
            var pathFinder = new PathFinder(matrix, startPoint, endPoint, heuristicCalculator);
            return pathFinder.FindPath().Count > 0;
        }
        
        private bool IsValidGridTile(GridTile tile)
        {
            return tile.GetGridObjectType() != GridObjectType.Obstacle &&
                   tile.GetGridObjectType() != GridObjectType.Matched;
        }
        
        private bool HasValidSequence(Vector2Int point, Vector2Int direction, int requiredCount)
        {
            int validCount = 0;
            int rows = _matrix.GetLength(0);
            int columns = _matrix.GetLength(1);
            Vector2Int currentPoint = point;

            while (currentPoint.x >= 0 && currentPoint.x < rows && currentPoint.y >= 0 && currentPoint.y < columns)
            {

                if (!IsValidGridTile(_matrix[currentPoint.x, currentPoint.y]))
                {
                    break; 
                }

                validCount++;
                if (validCount >= requiredCount)
                {
                    return true;
                }

                currentPoint += direction;
            }

            return false;
        }
        
        private bool HasPotentialMatchesInMatrix()
        {
            int rows = _matrix.GetLength(0);
            int columns = _matrix.GetLength(1);

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (HasValidSequence(new Vector2Int(x, y), new Vector2Int(0, 1), 3) || // Horizontal
                        HasValidSequence(new Vector2Int(x, y), new Vector2Int(1, 0), 3))   // Vertical
                    {
                        return true; 
                    }
                }
            }

            return false; 
        }

        public bool HasPotentialMatches()
        {
            if (HasPotentialMatchesInMatrix())
            {
                return true;
            }

            IHeuristicCalculator heuristicCalculator = new HeuristicCalculator();
            if (IsValidPath(ref _matrix, ref _startPoint, ref _endPoint, heuristicCalculator))
            {
                return true;
            }


            return false;
        }
    }
}
