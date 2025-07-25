using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Visual
{
    [CreateAssetMenu(fileName = "ParticleContainer", menuName = "PuzzleGame/ParticleContainer", order = 0)]
    public class ParticleContainer : SerializedScriptableObject
    {
        #region Public Fields

        [OdinSerialize]
        public Dictionary<int, GameObject> projectileHitEffectDictionary;

        #endregion

        #region Public Methods

        public GameObject GetParticle(int _index)
        {
            try
            {
                return projectileHitEffectDictionary.GetValueOrDefault(_index);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e + "Cannot find projectile hit effect with index: " + _index);
                throw;
            }
        }

        #endregion
    }
}