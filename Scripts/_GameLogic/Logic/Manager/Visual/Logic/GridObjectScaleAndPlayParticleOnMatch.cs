using System;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid_Object;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders;
using DG.Tweening;
using Handler.Extensions;
using Lean.Pool;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class GridObjectScaleAndPlayParticleOnMatch : MonoBehaviour, IMatchableAction
    {
        #region Private Variables
        
        public GameObject _defaultObject;
        public GameObject _raycastObject;
        
        private Action _OnScaleTweenEnded { get; set; }
        private Action _OnEndGameTweenEnded { get; set; }
        private Sequence _scaleSequence;
        private Sequence _endGameSequence;

        private const float _scalingFactor = 1.2f;
        private const float _duration = 0.25f;
        private GameObject _gridObject;
        
        #endregion

        #region Inspector Variables

        [SerializeField] private ParticleCallbackProvider _particleCallbackProvider;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            _scaleSequence = DOTween.Sequence();
          

        }

        private void OnEnable()
        {
            transform.localScale = Vector3.one;
            transform.DOKill();
        }
        
        
        private void OnDisable()
        {
            _scaleSequence.Kill();
            _endGameSequence.Kill();
            
        }
        

        #endregion

        #region Public Methods

        public void MatchAction(GridTile _gridTile, GridObjectType gridObjectType)
        {
            //AddOnEndCallback(gridObjectType);
           Debug.Log("MatchAction from " + _gridTile.GridPosition);


            var objectToAnimation = transform;
            _gridObject = _gridTile.GetGridObject().gameObject;

            _gridTile.SetGridObject(null);
_scaleSequence.Kill();
            //Destroy(objectToAnimation.gameObject);
        
            

            
            
            objectToAnimation.DOScale(1.3f, 0.2f).OnComplete(() =>
            {
                objectToAnimation.DOScale(0f, 0.2f).OnComplete(() =>
                {
                    _particleCallbackProvider.PlayParticles(gridObjectType, _gridObject.transform);
                    
                 

                }).SetLink(objectToAnimation.gameObject);
            }).SetEase(Ease.InOutCirc).SetLink(objectToAnimation.gameObject);
            
            /*_defaultObject.SetActive(false);
            _raycastObject.SetActive(true);
            
            _raycastObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            _raycastObject.transform.DOLocalMoveY(2, 0.5f).SetEase(Ease.OutSine);
            _raycastObject.transform.DOMoveZ(-20, 5f).OnComplete(() =>
            {
                _raycastObject.SetActive(false);
            }).SetEase(Ease.InSine);*/

            // ExecuteMatchingScaleSequence(_gridTile.GetGridObject().transform, true);
        }

        public void ExcludePathFoundAction(GridTile gridTile, GridObjectType gridObjectType)
        {
          

            var objectToAnimation = transform;
            
         //   Destroy(objectToAnimation.gameObject);
          //  ExecuteMatchFoundSequence(true, objectToAnimation);
        }

        #endregion
        
        #region Private Methods
        
        private void ExecuteMatchingScaleSequence(Transform objectToAnimation, bool _callEndCallback)
        {
         _scaleSequence.Append(objectToAnimation.DOScale(_scalingFactor * Vector3.one, _duration)
                .SetEase(Ease.OutBounce).SetLink(gameObject));
            
            _scaleSequence.Append(objectToAnimation.DOScale(Vector3.one, _duration)
                .SetEase(Ease.OutBounce).SetLink(gameObject));
            
            _scaleSequence.AppendCallback(() =>
            {
                if (_callEndCallback)
                {
                    _OnScaleTweenEnded?.Invoke();

                    EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true, false);
                }
            });
            
        }

        private void ExecuteMatchFoundSequence(bool _callEndCallback, Transform objectToAnimate)
        {
            if (_endGameSequence == null || !_endGameSequence.IsActive() || _endGameSequence.IsComplete())
            {
                _endGameSequence = DOTween.Sequence();
            }
            
            _endGameSequence.Append(objectToAnimate.DOMoveY(0.7f, 0.3f)
                .SetEase(Ease.OutBounce).SetLink(gameObject));
            
            _endGameSequence.Append(ScaleInWorldSpaceSequence(objectToAnimate, _scalingFactor * Vector3.one, 0.3f)
                .SetEase(Ease.OutBounce)).SetLink(gameObject);
            
            _endGameSequence.Append(ScaleInWorldSpaceSequence(objectToAnimate, Vector3.zero, 0.3f)
                .SetEase(Ease.OutBounce).SetLink(gameObject));
            
            _endGameSequence.AppendCallback(() =>
            {
                if (_callEndCallback)
                {
                    _OnEndGameTweenEnded?.Invoke();
                    EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true, false);
                }
            });
        }
        
        private void AddOnEndCallback(GridObjectType type)
        {
            _OnScaleTweenEnded += () =>
            {
                _particleCallbackProvider.PlayParticles(type, transform);
                //_objectSpawnProvider.ObjectSpawnAnimation();
            };
        }
        
    

        private Tween ScaleInWorldSpaceSequence(Transform target, Vector3 targetWorldScale, float duration)
        {
            Vector3 currentWorldScale = target.lossyScale;
            Vector3 normalizedScaleChange = CalculateNormalizedScaleChange(currentWorldScale, targetWorldScale);

            Vector3 localScaleChange = target.InverseTransformVector(normalizedScaleChange);
            Vector3 targetLocalScale = target.localScale.MultipliedBy(localScaleChange);
            Vector3 absoluteTargetLocalScale = CalculateAbsoluteTargetLocalScale(targetLocalScale);

            return DOTween.To(() => target.localScale, x => target.localScale = x, absoluteTargetLocalScale, duration)
                .SetEase(Ease.InOutCirc)
                .SetLink(target.gameObject);
        }

        private Vector3 CalculateNormalizedScaleChange(Vector3 currentScale, Vector3 targetScale)
        {
            return new Vector3(
                NormalizeScale(currentScale.x, targetScale.x),
                NormalizeScale(currentScale.y, targetScale.y),
                NormalizeScale(currentScale.z, targetScale.z)
            );
        }

        private float NormalizeScale(float current, float target)
        {
            return current != 0 ? target / current : 1.0f;
        }

        private Vector3 CalculateAbsoluteTargetLocalScale(Vector3 targetLocalScale)
        {
            return new Vector3(
                Mathf.Abs(targetLocalScale.x),
                Mathf.Abs(targetLocalScale.y),
                Mathf.Abs(targetLocalScale.z)
            );
        }
        
        #endregion
    }
}