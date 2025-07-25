#if UNITY_EDITOR
using System;
using _Game.Scripts._GameLogic.Logic.Grid;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.EditorFunctions
{
    [CustomEditor(typeof(GridMatrixController))]
    public class EditorGridMatrixInspector : Editor
    {
        #region PRIVATE VARIABLES

        private GridMatrixController _gridMatrixController;
        private PropertyTree _propertyTree;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            _gridMatrixController = (GridMatrixController)target;
            _gridMatrixController.OnDrawCell += DrawColoredCell;
        }

        private void OnDisable()
        {
            if (_propertyTree != null)
            {
                _propertyTree.Dispose();
                _propertyTree = null; 
            }

            _gridMatrixController.OnDrawCell -= DrawColoredCell;
        }

        #endregion

        #region GUI METHODS

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_propertyTree == null) _propertyTree = PropertyTree.Create(serializedObject);

            _propertyTree.BeginDraw(true);
            _propertyTree.Draw(false);
            _propertyTree.EndDraw();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region PRIVATE METHODS

        private static Color GetColorBasedOnGridObjectType(GridTile gridTile)
        {
            var defaultColor = Color.black;

            if (gridTile == null) return defaultColor;

            return gridTile.gridObjectType switch
            {
                GridObjectType.Matched => Color.gray,
                GridObjectType.Blue => Color.blue,
                GridObjectType.Green => Color.green,
                GridObjectType.Red => Color.red,
                GridObjectType.Yellow => Color.yellow,
                _ => defaultColor
            };
        }

        private GridTile DrawColoredCell(Rect rect, GridTile value)
        {
            const float padding = 2f;
            var paddedRect = PreparePaddedRect(rect, padding);

            var cellColor = GetColorBasedOnGridObjectType(value);

            EditorGUI.DrawRect(paddedRect, cellColor);

            // Check if the value is in the pointsArray
            var pointType = GetGridMatrixPointType(value);
            if (pointType.HasValue)
            {
                var text = pointType == GridMatrixPointType.Start ? "Start" : "End";
                var style = GetLabelStyle();
                EditorGUI.LabelField(paddedRect, text, style);
            }

            return value;
        }

        private Rect PreparePaddedRect(Rect rect, float padding)
        {
            return new Rect(rect.x + padding, rect.y + padding, rect.width - padding * 2, rect.height - padding * 2);
        }

        private GUIStyle GetLabelStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
        }
        
        private GridMatrixPointType? GetGridMatrixPointType(GridTile tile)
        {
            GridMatrixPointType? type = null;
            var index = Array.IndexOf(_gridMatrixController._pointsArray, tile);
            if (index != -1) type = (GridMatrixPointType)index;
            return type;
        }

        #endregion
    }
}
#endif
