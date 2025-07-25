using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridMatrixController : SerializedMonoBehaviour
    {
        #region Public Variables
        
        [HideInInspector] public Vector2Int startPoint;
        
        [HideInInspector] public Vector2Int endPoint;
        
        public GridTile[] _pointsArray = new GridTile[2];

        public GridManager _gridManager;

        public GridTileListWrapper gridTileListWrapper = new GridTileListWrapper();

        [ShowInInspector, TableMatrix(DrawElementMethod = "DrawColoredCell", SquareCells = true)]
        public GridTile[,] matrix;
        
        public GridTile[,] Matrix
        {
            get => matrix;
            set
            {
                matrix = value;
                gridTileListWrapper.FromMatrix(matrix);
            }
        }

        [HideInInspector] public Func<Rect, GridTile, GridTile> OnDrawCell;

        #endregion

        #region PRIVATE VARIABLES

        private List<GridObject> _allGridObjects;

        #endregion

        private void Awake()
        {
            matrix = gridTileListWrapper.ToMatrix();
        }


        #region Private Methods

        private GridTile DrawColoredCell(Rect rect, GridTile value)
        {
            return OnDrawCell?.Invoke(rect, value);
        }

        private Vector2Int? FindGridPosition(GridTile tile)
        {
            for (var x = 0; x < Matrix.GetLength(0); x++)
            for (var y = 0; y < Matrix.GetLength(1); y++)
            {
                var currentTile = Matrix[x, y];
                if (currentTile == tile) return new Vector2Int(x, y);
            }

            return null;
        }


        private List<GridTile> EarlyReturnIfInvalidPath(GridTile startTile, GridTile endTile)
        {
            var path = new List<GridTile>();

            if (startTile == null || endTile == null || Matrix == null) return path;

            if (startTile.gridObjectType != GridObjectType.Matched ||
                endTile.gridObjectType != GridObjectType.Matched)
                return path;

            return null;
        }

        private void AddPoint(Vector2Int point, GridMatrixPointType pointType)
        {
            if (Matrix == null) return;
            var tile = Matrix[point.x, point.y];
            if (tile != null) _pointsArray[(int)pointType] = tile;
        }

        #endregion

        #region Public Methods
        
        public void SetGridObjects(List<GridObject> allGridObjects)
        {
            _allGridObjects = allGridObjects;
        }

        public List<GridTile> CheckMatrixIfThereIsAPathFromStartToEnd()
        {
            var startTile = _pointsArray[(int)GridMatrixPointType.Start];
            var endTile = _pointsArray[(int)GridMatrixPointType.End];

            var earlyReturnPath = EarlyReturnIfInvalidPath(startTile, endTile);
            if (earlyReturnPath != null) return earlyReturnPath;

            var startGridPosition = FindGridPosition(startTile);
            var endGridPosition = FindGridPosition(endTile);

            var pathFinder = new PathFinder(Matrix, startGridPosition, endGridPosition, new HeuristicCalculator());
            return pathFinder.FindPath();
        }
        
        public void CreateGridMatrix()
        {
            var width = _gridManager.gridConfig.gridWidth;
            var height = _gridManager.gridConfig.gridHeight;
            var tempMatrix = new GridTile[width, height];
    
            // Loop assuming width as x (columns) and height as y (rows)
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Adjust indexing if needed based on how _allGridObjects are stored
                    tempMatrix[x, y] = _allGridObjects[y + x * height]._gridTile;
                    tempMatrix[x, y].SetGridPosition(new Vector2Int(x, y));
                }
            }

            Matrix = tempMatrix;
        }

        public void GeneratePoints()
        {
            _pointsArray = new GridTile[2];

            AddPoint(endPoint, GridMatrixPointType.Start);
            AddPoint(startPoint, GridMatrixPointType.End);
        }
        
        public void SetStartPoint(Vector2Int point)
        {
            startPoint = point;
        }
        
        public void SetEndPoint(Vector2Int point)
        {
            endPoint = point;
        }
        
        public int GetWidth()
        {
            return matrix.GetLength(0);
        }
        
        public int GetHeight()
        {
            return matrix.GetLength(1);
        }
        
        

        #endregion
    }
}

[Serializable]
public enum GridMatrixPointType
{
    Start,
    End
}