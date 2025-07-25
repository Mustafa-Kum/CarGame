using System;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridTile : MonoBehaviour
    {
        #region Public Variables

        public GridObjectType gridObjectType
        {
            get
            {
                if (gridObject == null)
                {
                    return GridObjectType.Matched;
                }
                else return gridObject.GridObjectType;

            }

            set
            {
                gridObject.GridObjectType = value;
            }
        }

        [SerializeField] private GridObject gridObject;


        public GridObject GetGridObject()
        {
            return gridObject;
        }

        public void SetGridObject(GridObject _gridObject)
        {
            gridObject = _gridObject;
        }

        public Transform GetRotatableObjectTransform()
        {
            return gridObject._rotatableObjectTransform;
        }


        public Vector2Int gridPosition;

        public bool HasBeenMatched { get; set; }

        #endregion

        #region Properties

        public GameObject GridObjectVariant { get; set; }

        private GridObjectType GridObjectType
        {
            get
            {
                return gridObject.GridObjectType;
            }
            set
            {
                gridObject.GridObjectType = value;
            }
        }

        public Vector2Int GridPosition
        {
            get
            {
                return gridPosition;
            }
            set
            {
                gridPosition = value;
            }
        }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get => gridObject == null;
            set => gridObject = value ? null : gridObject;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            gridObject = GetComponentInChildren<GridObject>();
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.PuzzleGameEnd += DeactivateGrid;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.PuzzleGameEnd -= DeactivateGrid;
        }

        #endregion
        
        private void DeactivateGrid()
        {
            transform.DOScale(Vector3.zero, 3).OnComplete(()=>
            {
                transform.gameObject.SetActive(false);
            });
        }

        public bool IsMatched()
        {
            return gridObjectType == GridObjectType.Matched || IsEmpty;
        }
        public void SetGridPosition(Vector2Int _gridPosition)
        {
            GridPosition = _gridPosition;
        }

        public void SetGridObjectType(GridObjectType _gridObjectType)
        {
            if(gridObject == null)
            {
                return;
            }
            gridObjectType = _gridObjectType;
            gridObject.GridObjectType = _gridObjectType;
        }

        public void SetGridObjectVariant(GameObject _gridObjectVariant)
        {
            GridObjectVariant = _gridObjectVariant;
        }

        public GridObjectType GetGridObjectType()
        {
            return gridObjectType;
        }

        public void MatchableAction()
        {
            if (gridObjectType == GridObjectType.Obstacle)
            {
                return;
            }
            
            EventManager.GridEvents.GridObjectMatchedType?.Invoke(GetGridObjectType(), transform.position);
            var prevGridObjectType = gridObjectType;
            gridObject?.GridObjectController.PerformMatchableAction(this, prevGridObjectType);

//            LeanPool.Despawn(gridObject.gameObject,2);
            
            IsEmpty = true;
            SetGridObjectType(GridObjectType.Matched);
            
            Debug.Log("Matchable" + gridPosition);

        }

        public void ConnectedAction()
        {
            Debug.Log("Connected");

            gridObject.GridObjectController.PerformConnectedAction(this);
        }

        public void PathAction()
        {
            gridObject.GridObjectController.PerformPathAction(this);
        }

        public void HiglightAction()
        {
//            Debug.Log("Highlight");

            gridObject.GridObjectController.PerformHighlightAction(this);
        }
        public void DeactivateAction()
        {
            gridObject?.GridObjectController.PerformDehighlightAction(this);
        }
        public void ClearGridType()
        {
            gridObjectType = GridObjectType.None;
            gridObject.GridObjectType = GridObjectType.None;
        }
        public void SetRotatableObject(Transform getRotatableObject)
        {
            gridObject._rotatableObjectTransform = getRotatableObject;

        }
        public void BackToNormalAction()
        {
            if (gridObject?.GridObjectController != null)
                gridObject?.GridObjectController.PerformBackToNormal(this);
        }
        public bool IsObstacle()
        {
            return gridObjectType == GridObjectType.Obstacle;
        }
        public void SetToBomb(GridObject _gridObject)
        {
            gridObject = _gridObject;
            gridObjectType = GridObjectType.Bomb;
        }

    }
}