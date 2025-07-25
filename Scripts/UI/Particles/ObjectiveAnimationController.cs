using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Logic.Manager.Objective;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Predefined;
using AssetKits.ParticleImage;
using DG.Tweening;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.UI.Particles
{
    public class ObjectiveAnimationController : SerializedMonoBehaviour
    {
        #region Inspector Fields

        public UIAnimationPrefabSO UIAnimationPrefabSo;

        public Canvas UICanvas;



        #endregion

        #region Private Variables

        private Sequence _sequence;

        private RectTransform _uiCanvasRectTransform;
        
        public Camera _mainCamera;

        public LevelProgressionItems _currentLevelObjectives;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _uiCanvasRectTransform = UICanvas.GetComponent<RectTransform>();
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            EventManager.GridEvents.GridObjectMatchedType += AnimateCoroutine;
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated += FetchLevelProgressItems;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.GridObjectMatchedType -= AnimateCoroutine;
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated -= FetchLevelProgressItems;
        }

        #endregion

        #region Private Methods

        private ParticleImage CreateParticleImage(GridObjectType type, Vector2 position)
        {
            var _viewTransform = _uiCanvasRectTransform;

            if (_viewTransform == null) return null;

            var _uiPrefab = UIAnimationPrefabSo.GetParticleImagePrefab(type);
            
            var particleImage = LeanPool.Spawn(_uiPrefab, position, Quaternion.identity, _viewTransform)
                .GetComponent<ParticleImage>();

          particleImage.attractorTarget = _uiCanvasRectTransform;

            return particleImage;
        }


        private void AnimateCoroutine(GridObjectType type, Vector3 worldPosition)
        {
            if (!_currentLevelObjectives.Objectives.Any(o => o.Type == type && o.RequiredCount > 0)) return;
            {
                var canvasPosition = ConvertWorldPositionToCanvasPosition(worldPosition);
            
                var currentParticleImage = CreateParticleImage(type, canvasPosition);

                if (currentParticleImage == null) return;
                
                currentParticleImage.onLastParticleFinished.AddListener(() =>
                {
                    PunchScaleTarget(currentParticleImage.attractorTarget);
                    
                    LeanPool.Despawn(currentParticleImage.gameObject);
                });
            }
        }
        
        private void PunchScaleTarget(Transform targetTransform)
        {
            _sequence = DOTween.Sequence();
            
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.Match, true, false);
            
            _sequence.Append(targetTransform.DOScale(1.1f, 0.1f));
            _sequence.Append(targetTransform.DOScale(0.8f, 0.1f)).SetDelay(0.01f);
        }
        
        private Vector2 ConvertWorldPositionToCanvasPosition(Vector3 worldPosition)
        {
            Vector2 screenPoint = _mainCamera.WorldToScreenPoint(worldPosition);

            return screenPoint;
        }


        private void FetchLevelProgressItems(List<ObjectiveItem> items)
        {
            _currentLevelObjectives.Objectives = new List<ObjectiveItem>(items);
        }

        #endregion
    }
}

[Serializable]
public struct RectTransformData
{
    public RectTransform _viewTransform;

    public Transform _targetTransform;
}