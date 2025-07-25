using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Data.Grid_Object;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid._Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Predefined;
using _Game.Scripts.Template.GlobalProviders.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class GridObject : MonoBehaviour, IClickableAction, IMatchableAction, IPathAction
    {
        #region Public Variables

        [SerializeField] private Transform _vertexTransform;
        
        [SerializeField] private GridObjectType _gridObjectType;
        [SerializeField] private GameObject _tileMatcherPartical;
        [SerializeField] private List<GameObject> _tileBombPartical;
        [SerializeField] private GridObjectController _gridObjectController;
        public GridObjectController GridObjectController
        {
            get => _gridObjectController;
            set => _gridObjectController = value;
        }
        
        
        public Transform _rotatableObjectTransform;
        
        //GET SET
        public GridObjectType GridObjectType    
        {
            get => originalGridObjectType;
            set
            {
                _gridObjectType = value;
            }
        }
        
        public GridObjectColorData GridObjectColorData;
        
        public GridTile _gridTile;
        

        public UnityAction<GridObject> OnGridObjectUpdated;
        
        public LevelList_SO levelListSo;
        
        #endregion

        #region Private Variables

        private int _currentRotationIndex;

        private const float totalTweenTime = 0.3f;

        [FormerlySerializedAs("IsMatchBoostActive")] public bool IsGridTileMatchBoosterActive;
        
        public bool IsGridTileRowBoosterActive;
        
        public bool IsGridTileBombBoosterActive;

        private bool IsTutorialLevelLockedInput;

        private bool _isClicked;
        
        GridObjectType originalGridObjectType;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            originalGridObjectType = _gridObjectType;
            _gridObjectController =GetComponent<GridObjectController>();
        }

        private void OnEnable()
        {
            EventManager.InputEvents.MouseUp += ClickableActionUp;
            
            ResetOnEnable();
        }

        private void ResetOnEnable()
        {
            _isClicked = false;
            IsGridTileMatchBoosterActive = false;
            IsGridTileBombBoosterActive = false;
            
            if (_tileMatcherPartical != null)
                _tileMatcherPartical.SetActive(false);
        }

        private void OnDisable()
        {
            EventManager.InputEvents.MouseUp -= ClickableActionUp;

        }

        #endregion

        #region Private Methods


        public void ExecuteRowTileBoostActive()
        {
            if (_tileMatcherPartical != null)
                _tileMatcherPartical.SetActive(true);
        }
        
        public void ExecuteBombMatchedAction()
        {
            if (_tileMatcherPartical != null)
                _tileMatcherPartical.SetActive(true);
            
            Debug.Log("ExecuteBombMatchedAction");
        }

        private void ProcessPlayerMove()
        {
                _isClicked = true;
                
                EventManager.GridEvents.OnGridObjectClicked?.Invoke(this);
              //  AnimateThisGridObject();
            
        }

        #endregion
        
        
        public void ClickableActionDown()
        {

            ProcessPlayerMove();
        }

        public void ClickableActionHold()
        {
            if (IsGridTileMatchBoosterActive)
            {
                return;
            }
            
            if (IsGridTileBombBoosterActive)
            {
                return;
            }
            
            if (IsTutorialLevelLockedInput)
            {
                return;
            }
            
            if (_isClicked)
            {
                return;
            }

            ProcessPlayerMove();

        }
        
        public void ClickableActionUp()
        {
            _isClicked = false;
//            Debug.Log("Up");
        }

        public void SetGridTile(GridTile grid)
        {
            _gridTile = grid;
        }

        public void MatchAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.OnConnect, true, false);
            _gridObjectType = GridObjectType.Matched;
            OnGridObjectUpdated?.Invoke(this);
            
            //   GetComponent<BoxCollider>().enabled = false;
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
            _gridObjectType = GridObjectType.Matched;
            OnGridObjectUpdated?.Invoke(this);
        }
        
        public void PathAction()
        {
        }
        
        public void FallToTile(GridTile targetTile)
        {
            
            if (_gridObjectType == GridObjectType.Obstacle || targetTile==_gridTile ||targetTile==null)
            {
                return;
            }   
            
            //transform.DOKill();
            // Create a DoTween sequence for the falling animation
            Sequence fallSequence = DOTween.Sequence();

            targetTile.IsEmpty = false;
            _gridTile.IsEmpty = true;
            _gridTile.SetGridObject(null);
            _gridTile = targetTile;
            targetTile.SetGridObject(this);
            ResetGridObjectType();
            // Calculate the target position based on the target tile's position
            Vector3 targetPosition = targetTile.transform.position;
      
            TDebug.Log("Tile " + _gridTile.GridPosition + " is falling to " + targetTile.GridPosition);
            // Add the falling animation to the sequence
            fallSequence.Append(transform.DOMove(targetPosition, 0.2f).SetEase(Ease.InQuad));
            // Add any additional animations or effects to the sequence
            // For example, you can add a slight bounce effect at the end of the fall
            fallSequence.Append(transform.DOMoveZ(targetPosition.z + 0.1f, 0.1f).SetEase(Ease.OutQuad));
            fallSequence.Append(transform.DOMoveZ(targetPosition.z, 0.1f).SetEase(Ease.InQuad));
          
            // Update the GridTile and GridObject after the animation is complete
            fallSequence.OnComplete(() =>
            {
                UpdateVisual();
            });

            // Start the fall sequence
            fallSequence.Play();
        }
        
        public void ResetGridObjectType()
        {
            _gridObjectType = originalGridObjectType;
        }

        private void UpdateVisual()
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                _gridObjectType = originalGridObjectType;
            });
            Debug.Log("Update Visual" + _gridTile.GridPosition);
            // Assign a new visual to the GridObject based on the GridTile's type
            // You can use a switch statement or if-else conditions to determine the appropriate visual
            // For example:
            // if (_gridTile.Type == TileType.Red)
            // {
            //     GetComponent<SpriteRenderer>().sprite = redSprite;
            // }
            // else if (_gridTile.Type == TileType.Blue)
            // {
            //     GetComponent<SpriteRenderer>().sprite = blueSprite;
            // }
            // ... and so on
        }
    }
}