using System.Collections.Generic;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    [System.Serializable]
    public class GridTileListWrapper
    {
        public List<GridTile> flatList;
        public int width;
        public int height;

        // Convert 2D array to flat list
        public void FromMatrix(GridTile[,] matrix)
        {
            flatList = new List<GridTile>();
            width = matrix.GetLength(0);
            height = matrix.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    flatList.Add(matrix[x, y]);
                }
            }
        }

        // Convert flat list back to 2D array
        public GridTile[,] ToMatrix()
        {
            GridTile[,] matrix = new GridTile[width, height];
            for (int i = 0; i < flatList.Count; i++)
            {
                int x = i / height;
                int y = i % height;
                matrix[x, y] = flatList[i];
            }
            return matrix;
        }
    }
}