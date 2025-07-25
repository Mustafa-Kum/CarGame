using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using UnityEngine;

namespace Game.Scripts.GameLogic.Logic.Grid
{
    public class MatrixSearcherQueries
    {
        private readonly GridTile[,] _matrix;

        public MatrixSearcherQueries(GridTile[,] matrix)
        {
            _matrix = TransposeMatrix(matrix);
        }

        private GridTile[,] TransposeMatrix(GridTile[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var cols = matrix.GetLength(1);
            var transposedMatrix = new GridTile[cols, rows];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    transposedMatrix[j, i] = matrix[i, j];
                }
            }

            return transposedMatrix;
        }

        public List<GridTile> GetGridTilesByRowFromMatrix(int row)
        {
            var gridTiles = new List<GridTile>();
            for (var i = 0; i < _matrix.GetLength(1); i++)
            {
                gridTiles.Add(_matrix[row, i]);
            }
            return gridTiles;
        }

        public List<GridTile> GetGridTilesByColumnFromMatrix(int column)
        {
            var gridTiles = new List<GridTile>();
            for (var i = 0; i < _matrix.GetLength(0); i++)
            {
                gridTiles.Add(_matrix[i, column]);
            }
            return gridTiles;
        }

        public List<GridObject> GetGridObjectsByRowFromMatrix(int row)
        {
            var gridObjects = new List<GridObject>();
            for (var i = 0; i < _matrix.GetLength(1); i++)
            {
                var gridObject = _matrix[row, i].GetComponentInChildren<GridObject>();
                if (gridObject != null)
                {
                    gridObjects.Add(gridObject);
                }
            }
            return gridObjects;
        }

        public List<GridTile> GetAllGridTilesFromMatrix()
        {
            var gridTiles = new List<GridTile>();
            for (var i = 0; i < _matrix.GetLength(0); i++)
            {
                for (var j = 0; j < _matrix.GetLength(1); j++)
                {
                    gridTiles.Add(_matrix[i, j]);
                }
            }
            return gridTiles;
        }

        public List<GridObject> GetAllGridObjectsFromMatrix()
        {
            var gridObjects = new List<GridObject>();
            for (var i = 0; i < _matrix.GetLength(0); i++)
            {
                for (var j = 0; j < _matrix.GetLength(1); j++)
                {
                    var gridObject = _matrix[i, j].GetComponentInChildren<GridObject>();
                    if (gridObject != null)
                    {
                        gridObjects.Add(gridObject);
                    }
                }
            }
            return gridObjects;
        }





        public List<GridObject> GetPotentialConnectableTiles(GridObject clickedGridObject)
        {
            var gridObjects = new List<GridObject>();
            if (clickedGridObject == null)
            {
                return gridObjects;
            }

            var gridTile = clickedGridObject._gridTile;
            var x = gridTile.GridPosition.y; // Keep as per your coordinate system
            var y = gridTile.GridPosition.x; // Keep as per your coordinate system

            // Define the eight possible directions for adjacent tiles (including diagonals)
            int[,] directions =
            {
                {
                    1,
                    0
                },
                {
                    -1,
                    0
                },
                {
                    0,
                    1
                },
                {
                    0,
                    -1
                },
                {
                    1,
                    1
                },
                {
                    1,
                    -1
                },
                {
                    -1,
                    1
                },
                {
                    -1,
                    -1
                }
            };

            for (var i = 0; i < directions.GetLength(0); i++)
            {
                var dx = directions[i, 0];
                var dy = directions[i, 1];

                var neighborX = x + dx;
                var neighborY = y + dy;

                if (neighborX >= 0 && neighborX < _matrix.GetLength(0) && neighborY >= 0 && neighborY < _matrix.GetLength(1))
                {
                    var neighborTile = _matrix[neighborX, neighborY];
                    var neighborObject = neighborTile.GetGridObject();
                    if (neighborObject != null)
                    {
                        gridObjects.Add(neighborObject);
                    }
                }
            }

            return gridObjects;
        }

        public List<GridObject> GetAllConnectedTiles(GridObject startGridObject)
        {
            var connectedTiles = new List<GridObject>();
            var visited = new HashSet<GridObject>();
            RecursiveTileSearch(startGridObject, connectedTiles, visited, startGridObject.GridObjectType);
            return connectedTiles;
        }

        private void RecursiveTileSearch(GridObject currentGridObject, List<GridObject> connectedTiles, HashSet<GridObject> visited, GridObjectType originalType)
        {
            if (visited.Contains(currentGridObject))
            {
                return;
            }

            visited.Add(currentGridObject);
            connectedTiles.Add(currentGridObject);

            List<GridObject> neighbors = GetPotentialConnectableTiles(currentGridObject);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    if (neighbor.GridObjectType == GridObjectType.Bomb)
                    {
                        // Continue the search with the original type
                        RecursiveTileSearch(neighbor, connectedTiles, visited, originalType);
                    }
                    else if (neighbor.GridObjectType == originalType)
                    {
                        // Continue the search with the original type
                        RecursiveTileSearch(neighbor, connectedTiles, visited, originalType);
                    }
                }
            }
        }

        public bool IsAdjacent(GridTile gridTile1, GridTile gridTile2)
        {
            var x1 = gridTile1.GridPosition.y;
            var y1 = gridTile1.GridPosition.x;
            var x2 = gridTile2.GridPosition.y;
            var y2 = gridTile2.GridPosition.x;

            return Mathf.Abs(x1 - x2) <= 1 && Mathf.Abs(y1 - y2) <= 1;
        }





        public List<GridObject> GetExcludeConnectableTiles(List<GridObject> potentialConnectables)
        {
            List<GridObject> allGridObjects = GetAllGridObjectsFromMatrix();
            allGridObjects.RemoveAll(potentialConnectables.Contains);
            return allGridObjects;
        }

        public List<GridTile> GetEmptyTilesBelow(GridTile tile)
        {
            var emptyTilesBelow = new List<GridTile>();
            var currentRow = tile.GridPosition.x;
            var currentCol = tile.GridPosition.y;

            for (var row = currentRow + 1; row < _matrix.GetLength(0); row++)
            {
                var belowTile = _matrix[row, currentCol];
                if (belowTile != null && (belowTile.IsEmpty || belowTile.IsMatched()))
                {
                    emptyTilesBelow.Add(belowTile);
                }
                else if (belowTile != null && belowTile.GetGridObjectType() == GridObjectType.Obstacle)
                {
                    break;
                }
            }

            return emptyTilesBelow;
        }

        public GridTile GetEmptyTileNeighborBelow(GridTile currentTile)
        {
            var rowOffsets = new[]
            {
                1,
                -1
            };
            var currentRow = currentTile.GridPosition.x;
            var currentCol = currentTile.GridPosition.y;

            foreach (var offset in rowOffsets)
            {
                var newRow = currentRow + offset;
                if (newRow < 0 || newRow >= _matrix.GetLength(0))
                {
                    continue;
                }

                for (var row = newRow + 1; row < _matrix.GetLength(0); row++)
                {
                    var belowTile = _matrix[row, currentCol];
                    if (belowTile != null && (belowTile.IsEmpty || belowTile.IsMatched()))
                    {
                        if (currentCol + 1 < _matrix.GetLength(1))
                        {
                            var rightNeighborTile = _matrix[row, currentCol + 1];
                            if (rightNeighborTile == null || rightNeighborTile.GetGridObjectType() == GridObjectType.Obstacle || rightNeighborTile.IsEmpty)
                            {
                                return belowTile;
                            }
                        }
                    }
                }
            }

            return null;
        }
        public GridTile GetGridTileAtPosition(int row, int col)
        {
            if (row < 0 || row >= _matrix.GetLength(0) || col < 0 || col >= _matrix.GetLength(1))
            {
                return null;
            }

            return _matrix[row, col];
        }

        public GridTile GetLowestEmptyTileInColumn(int column)
        {
            for (var row = _matrix.GetLength(0) - 1; row >= 0; row--)
            {
                var tile = _matrix[row, column];
                if (tile.IsEmpty)
                {
                    return tile;
                }
            }
            return null;
        }

        public GridTile GetTileAbove(GridTile tile)
        {
            var row = tile.GridPosition.x;
            var column = tile.GridPosition.y;
            if (column > 0)
            {
                return _matrix[row, column - 1];
            }
            return null;
        }
        public List<GridTile> GetAdjacentTiles(GridTile bombTile)
        {
            var row = bombTile.GridPosition.y;
            var column = bombTile.GridPosition.x;
            var adjacentTiles = new List<GridTile>();
            if (row > 0)
            {
                adjacentTiles.Add(_matrix[row - 1, column]);
            }
            if (row < _matrix.GetLength(0) - 1)
            {
                adjacentTiles.Add(_matrix[row + 1, column]);
            }
            if (column > 0)
            {
                adjacentTiles.Add(_matrix[row, column - 1]);
            }
            if (column < _matrix.GetLength(1) - 1)
            {
                adjacentTiles.Add(_matrix[row, column + 1]);
            }
            return adjacentTiles;
        }
    }
}
