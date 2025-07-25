using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Input;
using DG.Tweening;
using Game.Scripts.GameLogic.Logic.Grid;
using Lean.Pool;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridConnectionManager : MonoBehaviour
    {
        #region Public Variables

        public GridMatrixController gridMatrixController;
        public GridObjectContainer gridObjectContainer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float yOffsetOfLineRenderer = 1;
        private const float FallingDuration = 0.1f;

        #endregion

        #region Private Variables

        private PotentialMatchSearcher _potentialMatchSearcher;
        private bool _isProcessingPath;
        private MatrixSearcherQueries _matrixSearcherQueries;

        private readonly List<GridTile> _connectedTiles = new();
        private List<GridObject> _potentialConnectables = new();
        private readonly int _connectedCountForBomb = 5;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void Start()
        {
            Initialize();
        }

        #endregion

        #region Event Handlers

        private void HandleGridObjectClicked(GridObject clickedGridObject)
        {
            if (!_isProcessingPath && clickedGridObject.GridObjectType != GridObjectType.Bomb)
                ProcessGridObjectClick(clickedGridObject);
            else if (_potentialConnectables.Contains(clickedGridObject._gridTile.GetGridObject()) ||
                     (clickedGridObject._gridTile.GetGridObjectType() == GridObjectType.Bomb &&
                      _connectedTiles.Count > 0))
                UpdateConnectedGridObjects(clickedGridObject._gridTile.GetGridObject());
        }

        private void OnMouseUp()
        {
            ProcessMouseUp();
        }

        private void OnBoosterAnimationEnd()
        {
            InitiateFalling();
        }

        #endregion


        private void SubscribeToEvents()
        {
            EventManager.GridEvents.OnGridObjectClicked += HandleGridObjectClicked;
            EventManager.InputEvents.MouseUp += OnMouseUp;
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.GridEvents.OnGridObjectClicked -= HandleGridObjectClicked;
            EventManager.InputEvents.MouseUp -= OnMouseUp;
        }


        // Assuming other parts of your script are here...

        private void Initialize()
        {
            _matrixSearcherQueries = new MatrixSearcherQueries(gridMatrixController.Matrix);
            _potentialMatchSearcher = new PotentialMatchSearcher(gridMatrixController.Matrix,
                gridMatrixController.startPoint, gridMatrixController.endPoint);

            lineRenderer.positionCount = 0;
        }

        private void ProcessGridObjectClick(GridObject clickedGridObject)
        {
            _isProcessingPath = true;
            _potentialConnectables =
                _matrixSearcherQueries.GetAllConnectedTiles(clickedGridObject._gridTile.GetGridObject());

            _connectedTiles.Clear();
            _connectedTiles.Add(clickedGridObject._gridTile);

            UpdateLineRenderer(clickedGridObject._gridTile.transform.position);

            foreach (var grid in _potentialConnectables)
                grid._gridTile.HiglightAction();

            _matrixSearcherQueries.GetExcludeConnectableTiles(_potentialConnectables).ForEach(grid =>
            {
                if (grid != null) grid._gridTile.DeactivateAction();
            });
        }

        private void UpdateConnectedGridObjects(GridObject clickedGridObject)
        {
            var currentTile = clickedGridObject._gridTile;
            if (IsAdjacentToLastConnectedTile(currentTile))
            {
                if (!_connectedTiles.Contains(currentTile))
                {
                    _connectedTiles.Add(currentTile);
                    currentTile.ConnectedAction();
                }

                if (_connectedTiles.Count > 1)
                {
                    UpdateLineRenderer(currentTile.transform.position);
                    AnimatePreviousTile();
                }
            }
        }

        private bool IsAdjacentToLastConnectedTile(GridTile currentTile)
        {
            return _connectedTiles.Count == 0 ||
                   _matrixSearcherQueries.IsAdjacent(_connectedTiles[_connectedTiles.Count - 1], currentTile);
        }

        private void UpdateLineRenderer(Vector3 position)
        {
            var currentPositionCount = lineRenderer.positionCount;

            if (currentPositionCount == 0)
            {
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, new Vector3(position.x, yOffsetOfLineRenderer, position.z));
            }
            else
            {
                var newPositionIndex = currentPositionCount;

                lineRenderer.positionCount = currentPositionCount + 1;
                lineRenderer.SetPosition(newPositionIndex, lineRenderer.GetPosition(newPositionIndex - 1));

                UpdateLineRendererPosition(newPositionIndex,
                    new Vector3(position.x, yOffsetOfLineRenderer, position.z));
            }
        }

        private void UpdateLineRendererPosition(int index, Vector3 targetPosition)
        {
            DOTween.To(() => lineRenderer.GetPosition(index),
                    x => SetLineRendererPosition(index, x),
                    targetPosition,
                    0.2f)
                .SetEase(Ease.Linear);
        }

        private void SetLineRendererPosition(int index, Vector3 position)
        {
            if (index >= 0 && index < lineRenderer.positionCount) lineRenderer.SetPosition(index, position);
        }

        private void AnimatePreviousTile()
        {
            var previousTileTransform = _connectedTiles[_connectedTiles.Count - 2].GetGridObject().transform;
            previousTileTransform.DOKill();
            previousTileTransform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                previousTileTransform.DOPunchScale(new Vector3(0.03f, 0.03f, 0.03f), 0.5f, 1, 0.5f).SetLoops(-1);
            });
        }

        private void ProcessMouseUp()
        {
            ResetLineRenderer();
            if (_connectedTiles.Count > 1 && _connectedTiles.Count < _connectedCountForBomb)
                ProcessNormalMatches();
            else if (_connectedTiles.Count >= _connectedCountForBomb)
                ProcessBombCreation();
            else if (_connectedTiles.Count == 1) ResetConnectedTiles();
        }

        private void ProcessNormalMatches()
        {
            // List to hold all tiles to be matched, including adjacent bomb tiles
            var tilesToMatch = new List<GridTile>(_connectedTiles);

            // Check for bombs in all connected tiles
            var bombTiles = new List<GridTile>();
            foreach (var tile in _connectedTiles)
                if (tile.GetGridObject()?.GridObjectType == GridObjectType.Bomb)
                    bombTiles.Add(tile);

            // If there are bomb tiles, add their adjacent tiles to tilesToMatch
            if (bombTiles.Count > 0)
            {
                foreach (var bombTile in bombTiles)
                {
                    var adjacentTiles = _matrixSearcherQueries.GetAdjacentTiles(bombTile);
                    foreach (var adjTile in adjacentTiles)
                        if (!tilesToMatch.Contains(adjTile))
                        {
                            tilesToMatch.Add(adjTile);
                            Debug.Log("Bomb Adjacent Tile Added" + adjTile.GridPosition);
                        }
                }

                Debug.Log("Bomb Matched");

                EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.Placing, true, true);

                tilesToMatch.ForEach(tile => tile.GetGridObject()?.ExecuteBombMatchedAction());
            }

            // Process all matches

            tilesToMatch.ForEach(tile => tile.MatchableAction());


            ResetLineRenderer();
            InitiateFalling();
            ResetConnectedTiles();
        }

        private void ProcessBombCreation()
        {
            var lastTile = _connectedTiles[_connectedTiles.Count - 1];
            var lastTilePosition = lastTile.transform.position;

            foreach (var tile in _connectedTiles)
            {
                if (tile.GetGridObject().GridObjectType == GridObjectType.Bomb)
                {
                    var adjacentTiles = _matrixSearcherQueries.GetAdjacentTiles(tile);

                    adjacentTiles.ForEach(adjTile =>
                    {
                        if (!_connectedTiles.Contains(adjTile))
                            DOVirtual.DelayedCall(.5f, () =>
                            {
                                adjTile.GetGridObject()?.ExecuteBombMatchedAction();
                                adjTile.MatchableAction();
                                tile.GetGridObject()?.ExecuteBombMatchedAction();
                                tile.MatchableAction();

                                EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.Placing, true, true);
                            });
                    });


                    continue;
                }

                tile.GetGridObject().transform.DOMove(lastTilePosition, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    tile.MatchableAction();
                });
            }

            DOVirtual.DelayedCall(1f, () =>
            {
                _connectedTiles.Clear();

                var bombGO = LeanPool.Spawn(gridObjectContainer.GetBombGridObject().gameObject, lastTilePosition,
                    Quaternion.identity, lastTile.transform);
                var bombGridObject = bombGO.GetComponent<GridObject>();
                bombGO.transform.localScale = Vector3.zero;
                bombGO.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);

                DOVirtual.DelayedCall(0.5f, () =>
                {
                    lastTile.SetToBomb(bombGridObject);
                    bombGridObject.SetGridTile(lastTile);
                    lastTile.BackToNormalAction();
                    InitiateFalling();
                });
            });


            ResetLineRenderer();
            ResetConnectedTiles();
        }

        private void ProcessBombMatches(List<GridTile> bombTiles)
        {
            foreach (var bombTile in bombTiles)
            {
                var adjacentTiles = _matrixSearcherQueries.GetAdjacentTiles(bombTile);
                foreach (var adjTile in adjacentTiles) adjTile.MatchableAction();
                bombTile.MatchableAction();
            }

            // Ensure falling is only initiated once
            // (Moved InitiateFalling to the end of ProcessNormalMatches to ensure it runs once after all matches are processed)
        }


        private void ResetConnectedTiles()
        {
            _connectedTiles.ForEach(tile => tile.DeactivateAction());
            _connectedTiles.Clear();
            _potentialConnectables.ForEach(grid => grid._gridTile.BackToNormalAction());
            _potentialConnectables.Clear();
            _isProcessingPath = false;
        }

        private void ResetLineRenderer()
        {
            lineRenderer.positionCount = 0;
        }

        private void InitiateFalling()
        {
            var gridMatrix = gridMatrixController.Matrix;
            var numRows = gridMatrix.GetLength(0);
            var numColumns = gridMatrix.GetLength(1);
            var tilesFell = false;

            for (var col = 0; col < numColumns; col++)
            for (var row = 0; row < numRows; row++)
            {
                var currentTile = _matrixSearcherQueries.GetGridTileAtPosition(row, col);

                if (!currentTile.IsEmpty || currentTile.IsObstacle()) continue;

                var nextNonEmptyRow = row + 1;
                while (nextNonEmptyRow < numRows &&
                       (_matrixSearcherQueries.GetGridTileAtPosition(nextNonEmptyRow, col).IsEmpty ||
                        _matrixSearcherQueries.GetGridTileAtPosition(nextNonEmptyRow, col).IsObstacle()))
                    nextNonEmptyRow++;

                if (nextNonEmptyRow < numRows)
                {
                    var nonEmptyTile = _matrixSearcherQueries.GetGridTileAtPosition(nextNonEmptyRow, col);
                    var gridObject = nonEmptyTile.GetGridObject();
                    if (gridObject != null)
                    {
                        gridObject.FallToTile(currentTile);
                        currentTile.SetGridObject(gridObject);
                        nonEmptyTile.SetGridObject(null);

                        row--;

                        tilesFell = true;
                    }
                }
            }

            for (var row = numRows - 1; row >= 0; row--)
            for (var col = 0; col < numColumns; col++)
            {
                var currentTile = _matrixSearcherQueries.GetGridTileAtPosition(row, col);

                if (currentTile.IsEmpty && !currentTile.IsObstacle())
                {
                    if (TryFillFromAdjacentTile(row, col, Direction.Down, ref tilesFell)) continue;
                    if (TryFillFromAdjacentTile(row, col, Direction.DownRight, ref tilesFell)) continue;
                    if (TryFillFromAdjacentTile(row, col, Direction.DownLeft, ref tilesFell)) continue;
                }
            }

            for (var col = 0; col < numColumns; col++)
                if (_matrixSearcherQueries.GetGridTileAtPosition(numRows - 1, col).IsEmpty &&
                    !_matrixSearcherQueries.GetGridTileAtPosition(numRows - 1, col).IsObstacle())
                    GenerateNewGridObjectAndFallDown(_matrixSearcherQueries.GetGridTileAtPosition(numRows - 1, col));

            if (tilesFell) DOVirtual.DelayedCall(FallingDuration, InitiateFalling);
        }

        private bool TryFillFromAdjacentTile(int row, int col, Direction direction, ref bool tilesFell)
        {
            var numRows = gridMatrixController.Matrix.GetLength(0);
            var numColumns = gridMatrixController.Matrix.GetLength(1);

            var adjacentRow = row;
            var adjacentCol = col;

            switch (direction)
            {
                case Direction.Down:
                    adjacentRow++;
                    break;
                case Direction.DownRight:
                    adjacentRow++;
                    adjacentCol++;
                    break;
                case Direction.DownLeft:
                    adjacentRow++;
                    adjacentCol--;
                    break;
            }

            if (adjacentRow < numRows && adjacentCol >= 0 && adjacentCol < numColumns)
            {
                var adjacentTile = _matrixSearcherQueries.GetGridTileAtPosition(adjacentRow, adjacentCol);
                if (!adjacentTile.IsEmpty && !adjacentTile.IsObstacle())
                {
                    var gridObject = adjacentTile.GetGridObject();
                    if (gridObject != null)
                    {
                        var currentTile = _matrixSearcherQueries.GetGridTileAtPosition(row, col);
                        gridObject.FallToTile(currentTile);
                        currentTile.SetGridObject(gridObject);
                        adjacentTile.SetGridObject(null);
                        tilesFell = true;
                        return true;
                    }
                }
            }

            return false;
        }

        private void GenerateNewGridObjectAndFallDown(GridTile currentTile)
        {
            var gridObject = gridObjectContainer.GetRandomColredGridObject();
            var gridObjectVariant = LeanPool.Spawn(gridObject.gameObject, currentTile.transform.position,
                Quaternion.identity, currentTile.transform);
            currentTile.IsEmpty = false;
            gridObjectVariant.SetActive(true);
            gridObjectVariant.GetComponent<ClickableObject>().canClick = true;
            gridObjectVariant.GetComponent<GridObject>().SetGridTile(currentTile);
            currentTile.SetGridObject(gridObjectVariant.GetComponent<GridObject>());
            gridObjectVariant.transform.localScale = Vector3.zero;
            gridObjectVariant.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
            gridObjectVariant.transform.DOMoveY(0, 0.1f).SetEase(Ease.InOutQuad);
        }

        internal enum Direction
        {
            Down,
            DownRight,
            DownLeft
        }
    }
}