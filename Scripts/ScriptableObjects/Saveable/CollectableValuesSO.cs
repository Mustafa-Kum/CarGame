using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using Sirenix.OdinInspector;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "CollectableValues", menuName = "ThisGame/CollectableValues", order = 1)]
        public class CollectableValuesSO : PersistentSaveManager<CollectableValuesSO>, IResettable
        {
            public Dictionary<CollectableType, int> collectableValues = new Dictionary<CollectableType, int>();

            public int GetValue(CollectableType type)
            {
                if (collectableValues.TryGetValue(type, out int value))
                {
                    return value;
                }
                return 0;
            }

            public void AddValue(CollectableType type, int addedValue)
            {
                if (collectableValues.ContainsKey(type))
                {
                    collectableValues[type] += addedValue;
                }
                else
                {
                    collectableValues.Add(type, addedValue);
                }
            }
            
            public void MultiplyValue(CollectableType type, int multiplier)
            {
                if (collectableValues.ContainsKey(type))
                {
                    collectableValues[type] *= multiplier;
                }
                else
                {
                    collectableValues.Add(type, multiplier);
                }
            }

            public bool SpendValue(CollectableType type, int spentValue)
            {
                if (collectableValues.TryGetValue(type, out int currentValue))
                {
                    if (currentValue >= spentValue)
                    {
                        collectableValues[type] = currentValue - spentValue;
                        return true;
                    }
                }

                return false;
            }
        }
    }

