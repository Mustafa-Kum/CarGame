#if UNITY_EDITOR
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Helper.Extensions.System;
using Firebase.Crashlytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.EditorFunctions
{
    public sealed class EditorGridObjectInitializer
    {
        #region PUBLIC METHODS

        public JArray ParseTileDataFromJson(TextAsset jsonTextAsset)
        {
            try
            {
                var levelData = JObject.Parse(jsonTextAsset.text);
                TDebug.LogGreen("JSON Parsed successfully.");

                var tiles = (JArray)levelData["tiles"];

                if (tiles == null)
                {
                    TDebug.LogError("Tiles array is null.");
                    return null;
                }

                return tiles;
            }
            catch (JsonReaderException e)
            {
                TDebug.LogError($"JSON Parsing Error: {e.Message}");
                Crashlytics.LogException(e);
                return null;
            }
        }
        
        public JArray ParseStartAndEndPointDataFromJson(TextAsset jsonTextAsset)
        {
            try
            {
                var levelData = JObject.Parse(jsonTextAsset.text);

                // Extracting Start Point and End Point data
                var startPoint = (JArray)levelData["Start Point"];
                var endPoint = (JArray)levelData["End Point"];

                if (startPoint == null || endPoint == null)
                {
                    TDebug.LogError("Start Point or End Point data is missing.");
                    return null;
                }
                
                return new JArray(startPoint, endPoint);
            }
            catch (JsonReaderException e)
            {
                TDebug.LogError($"JSON Parsing Error for Start and End Points: {e.Message}");
                Crashlytics.LogException(e);
                return null;
            }
        }
        
        public GridObject InstantiateGridObjectAsVariant(GridObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            
            var instantiateGridObject = PrefabUtility.InstantiatePrefab(prefab, parent) as GridObject;
            
            if (instantiateGridObject == null)
            {
                TDebug.LogError("Instantiated grid object is null.");
                return null;
            }

            var transform = instantiateGridObject.transform;
            transform.position = position;
            transform.rotation = rotation;

            return instantiateGridObject;
        }
        
        public void PlaceGridObject(GridObject spawnedGridObject, GridTile gridComponent)
        {
            if (spawnedGridObject == null || gridComponent == null)
            {
                TDebug.LogError("GridObject or GridTile is null.");
                return;
            }

            gridComponent.SetGridObjectVariant(spawnedGridObject.gameObject);
            gridComponent.SetGridObjectType(spawnedGridObject.GridObjectType);
        }

        public void SetupGridObjectLayer(GridObject gridObject, string layerName)
        {
            if (gridObject == null)
            {
                TDebug.LogError("GridObject is null.");
                return;
            }

            gridObject.gameObject.layer = LayerMask.NameToLayer(layerName);
        }

        #endregion
    }
}
#endif
