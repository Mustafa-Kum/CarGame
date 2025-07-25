using System;
using System.Security.Cryptography;
using DG.Tweening;
using Lean.Pool;
using Sirenix.Utilities;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class ParticleCallbackProvider : MonoBehaviour
    {
        [SerializeField] public GridObjectTypeColorSettings colorSettings;
        
        #region Public Variables

        [SerializeField] private new ParticleSystem[] particleSystem;

        #endregion
        
        #region Public Methods

        public void PlayParticles(GridObjectType gridObjectType, Transform objectToAnimation)
        {
            foreach (var _particleSystem in particleSystem)
            {
                _particleSystem.gameObject.SetActive(true);
                var main = _particleSystem.main;
                _particleSystem.transform.position = objectToAnimation.position;
     
                         //main.startColor = ConvertGridObjectTypeToColor(gridObjectType);
            _particleSystem.Play();
             
            }
            
            
            //LeanPool.Despawn(transform.parent.gameObject);

            OnParticleEndCallback(particleSystem[^1], () =>
            {
                objectToAnimation.DOKill();
                transform.parent.DOKill();
                objectToAnimation.localScale = Vector3.one;
                particleSystem.ForEach(particle => particle.gameObject.SetActive(false));
                LeanPool.Despawn(transform.parent.gameObject);
              //  Destroy(transform.parent.gameObject    );
                //Debug.Log("Despawned" + objectToAnimation.gameObject.name);
              
            });
        }

        private void OnDestroy()
        {
//            Debug.Log("Destroyed");
        }

        #endregion

        #region Private Methods

        private Color ConvertGridObjectTypeToColor(GridObjectType gridObjectType)
        {
            return gridObjectType switch
            {
                GridObjectType.Red => colorSettings.Red,
                GridObjectType.Green => colorSettings.Green,
                GridObjectType.Blue => colorSettings.Blue,
                GridObjectType.Yellow => colorSettings.Yellow,
                GridObjectType.Matched => colorSettings.Matched,
                _ => Color.white
            };
        }
        
        private void OnParticleEndCallback(ParticleSystem _particleSystem, Action callback)
        {
            var sequence = DOTween.Sequence();
            var main = _particleSystem.main;
            var duration = main.duration;
            sequence.AppendInterval(duration).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        #endregion
    }
}

[System.Serializable]
public class GridObjectTypeColorSettings
{
    public Color Red = Color.red;
    public Color Green = Color.green;
    public Color Blue = Color.blue;
    public Color Yellow = Color.yellow;
    public Color Matched = Color.cyan;
}