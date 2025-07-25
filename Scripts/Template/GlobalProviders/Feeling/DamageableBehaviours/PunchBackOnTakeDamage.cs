using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Template.GlobalProviders.Combat;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.DamageableBehaviours
{
    public class PunchBackOnTakeDamage : MonoBehaviour, IDamageableAction, IInteractableAction
    {
        #region Inspector Fields

        public Transform punchBackTransform;

        #endregion
        
        #region Private Methods

        private Tween _punchBackTween;
        
        private Sequence _punchBackSequence;

        #endregion
        
        #region Public Methods

        public void Initialize(DamageableObject damageableObject)
        {
            _punchBackSequence = DOTween.Sequence();
        }

        public void TakeDamage(float damage)
        {
            // Ensuring any ongoing punch back sequence is terminated
            if (_punchBackSequence != null)
            {
                _punchBackSequence.Kill();
            }

            // Store the original position
            Vector3 originalPosition = punchBackTransform.localPosition;

            // Start the forward punch back sequence
            _punchBackSequence = DOTween.Sequence()
                .Append(punchBackTransform.DOLocalMoveZ(originalPosition.z + 0.5f, 0.2f))
                .OnComplete(() => 
                {
                    // Move back to the original position after the forward tween completes
                    punchBackTransform.DOLocalMoveZ(originalPosition.z, 0.2f);
                });
        }


        public void HealthChanged(float currentHealth)
        {
        }

        public void Death()
        {
        }

        #endregion

        public void InteractableAction()
        {
            _punchBackSequence?.Kill();
            _punchBackTween?.Kill();
        }
    }
}