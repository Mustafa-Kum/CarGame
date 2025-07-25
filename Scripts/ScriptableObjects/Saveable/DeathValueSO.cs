﻿using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.RunTime;
using _Game.Scripts.Template.GlobalProviders.Combat;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "DeathValues", menuName = "ThisGame/DeathValues", order = 0)]
    public class DeathValueSO : PersistentSaveManager<DeathValueSO>, IResettable
    {
        #region Public Variables

        public Dictionary<DamageableData.DamageableType, int> deathValues = new Dictionary<DamageableData.DamageableType, int>();

        #endregion

        #region Public Methods

        public int GetValue(DamageableData.DamageableType type)
        {
            return deathValues.TryGetValue(type, out var value) ? value : 0;
        }
        
        public void AddValue(DamageableData.DamageableType type, int addedValue)
        {
            if (deathValues.ContainsKey(type))
            {
                deathValues[type] += addedValue;
            }
            else
            {
                deathValues.Add(type, addedValue);
            }
        }
        
        public void AddValueByOne(DamageableData.DamageableType type)
        {
            if (deathValues.ContainsKey(type))
            {
                deathValues[type] += 1;
            }
            else
            {
                deathValues.Add(type, 1);
            }
        }
        
        public void RemoveValue(DamageableData.DamageableType type, int removedValue)
        {
            if (deathValues.ContainsKey(type))
            {
                deathValues[type] -= removedValue;
            }
            else
            {
                deathValues.Add(type, -removedValue);
            }
        }
        
        #endregion
    }
}