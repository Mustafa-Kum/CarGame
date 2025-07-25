using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.RunTime;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "CollectableValues", menuName = "ThisGame/CollectableValues", order = 1)]
    public class CurrencyValuesSO : PersistentSaveManager<CurrencyValuesSO>, IResettable
    {
        public Dictionary<CollectableType, int> collectableValues = new();

        public int GetValue(CollectableType type) => collectableValues.GetValueOrDefault(type, 0);

        [Button]
        public void AddValue(CollectableType type, int addedValue)
        {
            if (collectableValues.ContainsKey(type))
                collectableValues[type] += addedValue;
            else
                collectableValues.Add(type, addedValue);
            
            EventManager.Resource.CurrencyChanged?.Invoke(type);
        }

        [Button]
        public void ClearCurrency(CollectableType type)
        {
            collectableValues[type] = 0;
            
            EventManager.Resource.CurrencyChanged?.Invoke(type);
        }

        public void MultiplyValue(CollectableType type, int multiplier)
        {
            if (collectableValues.ContainsKey(type))
                collectableValues[type] *= multiplier;
            else
                collectableValues.Add(type, multiplier);
        }

        public bool SpendValue(CollectableType type, int spentValue)
        {
            if (collectableValues.TryGetValue(type, out var currentValue))
                if (currentValue >= spentValue)
                {
                    collectableValues[type] = currentValue - spentValue;
                    return true;
                }

            return false;
        }
    }
}