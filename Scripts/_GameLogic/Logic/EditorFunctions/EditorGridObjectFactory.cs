using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Helper.Extensions.System;
using Firebase.Crashlytics;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.EditorFunctions
{ 
    public sealed class EditorGridObjectFactory
    {
#if UNITY_EDITOR

        #region PRIVATE VARIABLES

        private readonly GridConfig _gridConfig;
        private readonly bool _isSpawnFromJson;
        private readonly GridObjectContainer _gridObjectContainer;
        private const string GridLayerName = "Grid";
        private readonly EditorGridObjectInitializer _gridObjectInitializer;
        private readonly List<GridObject> AllGridObjects;
        private readonly GridMatrixController _gridMatrixController;

        #endregion
        
        #region CONSTRUCTORS
        
        public EditorGridObjectFactory(GridConfig gridConfig, bool isSpawnFromJson, GridObjectContainer gridObjectContainer,
            EditorGridObjectInitializer gridObjectInitializer, List<GridObject> allGridObjects = null, GridMatrixController gridMatrixController = null)
        {
            _gridConfig = gridConfig;
            _isSpawnFromJson = isSpawnFromJson;
            _gridObjectContainer = gridObjectContainer;
            _gridObjectInitializer = gridObjectInitializer;
            AllGridObjects = allGridObjects ?? new List<GridObject>();
            _gridMatrixController = gridMatrixController;
        }
        
        #endregion
        
        #region PUBLIC METHODS

        public void CreateGridObjectsFromJson(TextAsset jsonTextAsset)
        {
            try
            {
                ValidateGridConfig();
                var data = _gridObjectInitializer.ParseTileDataFromJson(jsonTextAsset);
                var points = _gridObjectInitializer.ParseStartAndEndPointDataFromJson(jsonTextAsset);
                if (data == null) return;

                foreach (var jToken in data) 
                    InitializeGridObject(jToken, points);

                GetAllGridObjectsAndAddToList(_gridConfig);
            }
            catch (Exception e)
            {
                TDebug.LogError($"Error in GenerateGridObjectsFromJson: {e.Message}");
                Crashlytics.LogException(e);
                throw;
            }
        }
        
        public void CreateGridObjectsRandom()
        {
            ValidateGridConfig();

            for (var i = 0; i < _gridConfig.gridWidth; i++)
            for (var j = 0; j < _gridConfig.gridHeight; j++)
                ProcessGridObject(i, j, null);

            GetAllGridObjectsAndAddToList(_gridConfig);
        }

        #endregion

        #region PRIVATE METHODS

        private void ValidateGridConfig()
        {
            if (_gridConfig.grid == null || _gridConfig.grid.Count == 0)
                throw new Exception("Grid is null or empty");
        }
        
        private GridObject GetGridObject(string gridObjectTypeStr)
        {
            if (_isSpawnFromJson && gridObjectTypeStr != null)
            {
                var gridObjectType = (GridObjectType)Enum.Parse(typeof(GridObjectType), gridObjectTypeStr);
                return _gridObjectContainer.GetGridObjectByType(gridObjectType);
            }

            return _gridObjectContainer.GetRandomGridObject();
        }
        
        private GridTile GetGridTile(int i, int j)
        {
            var index = j * _gridConfig.gridWidth + i;
            return _gridConfig.grid[index];
        }

        private void ProcessGridObject(int tileX, int tileY, string type)
        {
            ValidateParameters(tileX, tileY);

            var grid = GetGridTile(tileX, tileY);
            if (grid == null) throw new Exception($"Grid object is null at position [{tileX}, {tileY}]");

            var gridObject = GetGridObject(type);
            if (gridObject == null) return;

            var spawnedGridObject = SpawnGridObject(gridObject, grid);
            if (spawnedGridObject == null) return;

            FinalizeGridObjectProducing(spawnedGridObject, grid);
        }

        private void ValidateParameters(int i, int j)
        {
            if (_gridConfig == null)
                throw new ArgumentNullException(nameof(_gridConfig), "Grid config object is null.");
            if (_gridObjectInitializer == null)
                throw new ArgumentNullException(nameof(_gridObjectInitializer), "Grid object initializer is null.");

            if (i < 0 || i >= _gridConfig.gridWidth || j < 0 || j >= _gridConfig.gridHeight)
                throw new ArgumentOutOfRangeException("i" + i + "j" + j, "Grid object is out of range.");
        }

        private GridObject SpawnGridObject(GridObject gridObject, GridTile grid)
        {
            var transform = grid.transform;
            return _gridObjectInitializer.InstantiateGridObjectAsVariant(
                gridObject,
                transform.position,
                gridObject.transform.rotation,
                transform);
        }

        private void FinalizeGridObjectProducing(GridObject spawnedGridObject, GridTile grid)
        {
            spawnedGridObject.SetGridTile(grid);
            _gridObjectInitializer.PlaceGridObject(spawnedGridObject, grid);
            _gridObjectInitializer.SetupGridObjectLayer(spawnedGridObject, GridLayerName);
        }

        private void GetAllGridObjectsAndAddToList(GridConfig gridConfig)
        {
            var gridTiles = gridConfig.GetGridTiles();

            for (var index = 0; index < gridTiles.Count; index++)
            {
                var gridTile = gridTiles[index];
                var gridObject = gridTile.GetComponentInChildren<GridObject>();
                if (gridObject != null) AllGridObjects.Add(gridObject);
            }
        }

        private void InitializeGridObject(JToken jsonTileToken, JArray jsonPointsToken)
        {
            var tile = (JObject)jsonTileToken;
            var tileX = (int)tile["x"];
            var tileY = (int)tile["y"];
            var type = (string)tile["type"];

            var startPoint = jsonPointsToken[0][0]; 
            var endPoint = jsonPointsToken[1][0];

            var startX = (int)startPoint?["x"];
            var startY = (int)startPoint?["y"];
            var endX = (int)endPoint?["x"];
            var endY = (int)endPoint?["y"];
            
            var vectorStartPoint = new Vector2Int(startX, startY);
            var vectorEndPoint = new Vector2Int(endX, endY);
            
            _gridMatrixController.SetStartPoint(vectorStartPoint);
            _gridMatrixController.SetEndPoint(vectorEndPoint);

            ProcessGridObject(tileX, tileY, type);
        }

        #endregion
#endif
    }
}
