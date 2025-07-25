using System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders
{
    public abstract class PunchScaleProvider : MonoBehaviour
    {
        #region Private Variables

        private Tween _tween;
        
        private Vector3 originalScale;

        private Sequence localSequence;
        
        #endregion

        #region Inspector Variables

        [SerializeField] private Transform targetTransform;

        #endregion

        #region Inspector Variables

        [SerializeField] private float duration = 0.25f;

        [SerializeField] private float desiredScale;
        
        #endregion

        #region Unity Events

        private void Awake()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
            
            originalScale = targetTransform.localScale;
        }

        private void OnDestroy() => localSequence.Kill();

        #endregion

        #region Protected Methods

        protected virtual void PunchScale()
        {
            localSequence?.Kill();
            
            localSequence = DOTween.Sequence();
            
            Vector3 targetScale = originalScale + (Vector3.one * desiredScale); 
            localSequence.Append(transform.DOScale(targetScale, duration).SetEase(Ease.OutQuad));
            localSequence.Append(transform.DOScale(originalScale, duration).SetEase(Ease.InQuad));
    
            localSequence.OnComplete(() =>
            {
                transform.localScale = originalScale;
            });
    
            localSequence.Play();
        }

        #endregion
        
    }
}