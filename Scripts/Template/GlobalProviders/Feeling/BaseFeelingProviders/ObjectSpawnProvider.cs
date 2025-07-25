// ObjectSpawnProvider.cs
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.BaseFeelingProviders
{
    public class ObjectSpawnProvider : MonoBehaviour
    {
        #region Inspector Variables
        [Header("Damage Visuals")]
        [SerializeField] public GameObject _damageEffectPrefab;
        [SerializeField] public float _effectDuration = 1f;
        [SerializeField] public Transform initialPosition; // Initial position set via Inspector
        #endregion

        #region Protected Methods
        protected void CreateCurvyMovement(GameObject objectToMove, Vector3 endPosition)
        {
            var startPosition = initialPosition.position;
            endPosition.y = -0.5f;

            // Define a midpoint for the curve. Higher the Y value, more pronounced the curve.
            Vector3 midPoint = (startPosition + endPosition) / 2 + Vector3.up * 2.5f;

            Vector3[] path = new Vector3[] { midPoint, endPosition };

            objectToMove.transform.position = startPosition; // Start at the initial position
            objectToMove.transform.DOLocalPath(path, _effectDuration, PathType.CatmullRom)
                .SetLookAt(0.01f, Vector3.forward)
                .SetOptions(false)
                .SetEase(Ease.Linear).OnComplete(() => objectToMove.GetComponent<Rigidbody>().isKinematic = false);
        }

        protected GameObject DamageEffectObject()
        {
            GameObject damageEffectInstance = Instantiate(_damageEffectPrefab, initialPosition.position, 
                Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            return damageEffectInstance;
        }
        #endregion
    }
}