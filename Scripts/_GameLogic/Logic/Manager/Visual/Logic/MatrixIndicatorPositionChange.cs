using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class MatrixIndicatorPositionChange : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        private float yOffset = 9f;

        [SerializeField] private Vector3 startRotation;
        
        private Vector3 _initialRotation;
        
        private Vector3 _initialPosition;

        [SerializeField]
        private float moveDuration = 5f;

        #endregion
        
        #region UNITY METHODS

        private void OnEnable() => EventManager.InGameEvents.LevelStart += AnimateMovementAndRotation;

        private void OnDisable() => EventManager.InGameEvents.LevelStart -= AnimateMovementAndRotation;

        private void Awake()
        {
            CacheInitialPosition();
            CacheInitialRotation();
            SetInitialPosition();
            SetInitialRotation();
        }

        #endregion

        #region PRIVATE METHODS
        private void SetInitialPosition() => transform.position = new Vector3(0, yOffset, 0);
        
        private void CacheInitialRotation() => _initialRotation = transform.rotation.eulerAngles;
        
        private void CacheInitialPosition() => _initialPosition = transform.position;
        
        private void SetInitialRotation() => transform.rotation = Quaternion.Euler(startRotation);
        
        private void AnimateMovementAndRotation()
        {
            Sequence sequence = DOTween.Sequence().SetLink(gameObject);

            Quaternion targetRotation = Quaternion.Euler(_initialRotation);

            sequence.Append(transform.DOMove(_initialPosition, moveDuration).SetEase(Ease.OutQuad));
            sequence.Join(transform.DOLocalRotateQuaternion(targetRotation, moveDuration).SetEase(Ease.InOutQuad));

            sequence.Play();
        }

        #endregion
    }
}