using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class BulgeScalingTransformAnimation : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private List<Transform> transformsToScale = new();

        // Adjust these fields to control the scaling and animation
        public Vector3 scaleUpValue = new(1.5f, 1.5f, 1.5f);
        public float animationDuration = 1f;
        public float delayBetweenAnimations = 0.5f;

        #endregion

        #region PUBLIC METHODS

        [Button("Scale Transforms")]
        public void ScaleTransforms()
        {
            StartCoroutine(ScaleAllTransformsCoroutine());
        }

        #endregion


        #region PRIVATE METHODS

        [Button("Get All Children")]
        private void GetAllChildren()
        {
            transformsToScale.Clear();
            foreach (Transform child in transform) transformsToScale.Add(child);
            transformsToScale.Reverse();
        }

        private IEnumerator ScaleAllTransformsCoroutine()
        {
            foreach (var t in transformsToScale)
            {
                // Apply delay before starting the scale animation
                yield return new WaitForSeconds(delayBetweenAnimations);

                // Scale up and then return to original scale
                t.DOScale(scaleUpValue, animationDuration).SetLoops(2, LoopType.Yoyo);
            }
        }

        #endregion
    }
}