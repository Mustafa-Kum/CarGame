using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.InteractableBehaviours
{
    public class ParticleManagerOnCollect : SerializedMonoBehaviour
    {
        #region Public Variables

        public Dictionary<CollectableType, ParticleSystem> _collectableParticles;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        #endregion

        #region Shared Methods

        private void Subscribe()
        {
            EventManager.CollectableEvents.Collect += PlayParticle;
        }

        private void Unsubscribe()
        {
            EventManager.CollectableEvents.Collect -= PlayParticle;
        }

        #endregion

        #region Private Methods

        private void PlayParticle(CollectableData collectableData)
        {
            if (_collectableParticles.TryGetValue(collectableData.Type, out ParticleSystem particleSystemPrefab))
            {
                var spawnedParticleGameObject = LeanPool.Spawn(
                    particleSystemPrefab.gameObject.gameObject);
                

                var spawnedParticle = spawnedParticleGameObject.GetComponent<ParticleSystem>();
                spawnedParticle.Play();

                StartCoroutine(DespawnWhenDone(spawnedParticleGameObject, spawnedParticle.main.duration));
            }
        }

        private IEnumerator DespawnWhenDone(GameObject particleGameObject, float duration)
        {
            yield return new WaitForSeconds(duration);
            LeanPool.Despawn(particleGameObject);
        }

        #endregion
    }
}
