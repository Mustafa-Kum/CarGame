using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    [Serializable]
    public class GridConfig
    {
        #region Public Fields

        public int gridWidth;
        public int gridHeight;
        public List<GridTile> grid;
        
        #endregion

        #region Constructors

        public GridConfig(int width, int height)
        {
            gridWidth = width;
            gridHeight = height;
            grid = new List<GridTile>(width * height);

            for (int i = 0; i < width * height; i++)
            {
                grid.Add(null);
            }
        }

        #endregion

        #region Public Methods

        public void SetGridTile(Vector2Int position, GridTile obj)
        {
            int index = position.x * gridWidth + position.y;
            if (index >= 0 && index < grid.Count)
            {
                grid[index] = obj;
            }
            else
            {
                Debug.LogError($"Trying to set a GridTile at an out-of-bounds index: {index} for position {position}");
            }
        }
        
        public GridTile GetGridTile(Vector2Int value)
        {
            return grid[value.x * gridWidth + value.y];
        }
        
        public List<GridTile> GetGridTiles()
        {
            return grid;
        }
        
        public Vector2Int GetGridConfig()
        {
            int height = gridHeight;
            int width = gridWidth;
            return new Vector2Int(width, height);
        }
        
        #endregion
    }
}