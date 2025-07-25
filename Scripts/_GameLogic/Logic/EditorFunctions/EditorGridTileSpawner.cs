using System;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Manager;
using _Game.Scripts.Helper.Extensions.System;
using Firebase.Crashlytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.EditorFunctions
{
    public class EditorGridTileSpawner : MonoBehaviour
    {
#if UNITY_EDITOR

        #region Public Variables

        public GridManager gridManager;
        public GridTileContainer gridTileContainer;
        public EditorGridObjectManager editorGridObjectManager;
        public JSONGridSize jsonGridData;
        public float gap = 0.1f;
        
        #endregion

        #region Public Methods

        [Button]
        public void GenerateGrid()
        {
            ClearGrid();
            TryFetchGridDataFromJson();
            ConfigureGrid(6, 6);
            GenerateGridTiles(gap);
            editorGridObjectManager.SetupGridObjects(gridManager.gridConfig);
        }
        
        
        [Button]
        public void ClearGrid()
        {
            if (gridManager.gridConfig == null) return;
            foreach (var child in gridManager.gridConfig.grid) 
                DestroyImmediate(child.gameObject);

            gridManager.gridConfig.gridWidth = 0;
            gridManager.gridConfig.gridHeight = 0;
            gridManager.gridConfig.grid.Clear();
            editorGridObjectManager.AllGridObjects.Clear();
        }

        #endregion

        #region Private Methods

        private void TryFetchGridDataFromJson()
        {
            try
            {
                if (editorGridObjectManager.levelJsonFile == null) return;

                var gridSize = ParseGridSizeFromJson(editorGridObjectManager.levelJsonFile);
                if (!gridSize.HasValue) return; 

                jsonGridData.width = gridSize.Value.x;
                jsonGridData.height = gridSize.Value.y;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Crashlytics.LogException(e);
                throw;
            }
        }
        
        private void ConfigureGrid(int width, int height)
        {
            gridManager.gridConfig = new GridConfig(width, height);
        }

        #region Private Methods

        private void GenerateGridTiles(float gap)
        {
            Vector3 startPosition = ComputeStartPosition(gap);
            for (var x = 0; x < gridManager.gridConfig.gridWidth; x++)
            for (var z = 0; z < gridManager.gridConfig.gridHeight; z++)
            {
                GenerateTileAt(x, z, startPosition, gap);
            }
        }

        private Vector3 ComputeStartPosition(float gap)
        {
            return gridManager.transform.position -
                   new Vector3((gridManager.gridConfig.gridWidth - 1) * (1 + gap) / 2.0f, 0, (gridManager.gridConfig.gridHeight - 1) * (1 + gap) / 2.0f) +
                   new Vector3(0.5f, 0, 0.5f);
        }

        private void GenerateTileAt(int x, int z, Vector3 startPosition, float gap)
        {
            var position = startPosition + new Vector3(x * (1 + gap), 0, z * (1 + gap));
            var gridTileGameObject = PrefabUtility.InstantiatePrefab(gridTileContainer.GetGridPrefab(), gridManager.transform) as GameObject;

            if (gridTileGameObject == null) return;

            gridTileGameObject.layer = LayerMask.NameToLayer("Grid");
            gridTileGameObject.transform.position = position;
            var gridTileComponent = gridTileGameObject.GetComponent<GridTile>();

            if (gridTileComponent == null)
            {
                Debug.LogError("GridTile component not found on the instantiated grid prefab.");
                return;
            }

            gridManager.gridConfig.SetGridTile(new Vector2Int(x, z), gridTileComponent);
        }

        #endregion


        #endregion

        #region QUERY METHODS

        private Vector2Int? ParseGridSizeFromJson(TextAsset jsonTextAsset)
        {
            try
            {
                var levelData = JObject.Parse(jsonTextAsset.text);

                var gridSize = levelData["gridSize"] as JObject;
                if (gridSize == null)
                {
                    TDebug.LogError("Grid Size data is missing.");
                    return null;
                }

                int width = (gridSize["width"] ?? throw new InvalidOperationException()).Value<int>();
                int height = (gridSize["height"] ?? throw new InvalidOperationException()).Value<int>();

                return new Vector2Int(width, height);
            }
            catch (JsonReaderException e)
            {
                TDebug.LogError($"JSON Parsing Error for Grid Size: {e.Message}");
                Crashlytics.LogException(e);
                return null;
            }
        }

        #endregion
#endif
    }
    
    [Serializable]
    public struct JSONGridSize
    {
        public int width;
        public int height;
    }
}
