using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.RunTime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "HealthValuesSO", menuName = "PuzzleGame/HealthValues", order = 0)]
    public class HealthValuesSO : PersistentSaveManager<HealthValuesSO>, IResettable
    {
        #region PUBLIC VARIABLES

        [SerializeField] private int healthValue = 0;
        [SerializeField] private int maxHealthValue = 5;
        [SerializeField] private bool isInfiniteHealth = false;

        #endregion

        #region PROPERTIES

        public int MaxHealthValue => maxHealthValue;
        
        public int HealthValue => healthValue;
        
        public bool IsInfiniteHealth => isInfiniteHealth;
        
        #endregion
        
        #region PUBLIC METHODS
        
        public int GetHealthValue()
        {
            return HealthValue;
        }
        
        public bool IsHealthFull()
        {
            return HealthValue == MaxHealthValue;
        }
        
        public void DecreaseHealthValue(int decreasedValue)
        {
            if (isInfiniteHealth)
            {
                return;
            }
            
            if (decreasedValue <= 0)
            {
                return;
            }

            healthValue -= decreasedValue;
            
            if (healthValue < 0)
            {
                healthValue = 0;
            }
        }
        
        public void IncreaseHealthValue(int increasedValue)
        {
            if (isInfiniteHealth)
            {
                return;
            }
            
            if (isInfiniteHealth)
            {
                return;
            }
            
            if (increasedValue <= 0)
            {
                return;
            }

            healthValue += increasedValue;
            
            if (healthValue > maxHealthValue)
            {
                healthValue = maxHealthValue;
            }
        }
        
        public void SetInfiniteHealth(bool value)
        {
            isInfiniteHealth = value;
            
            EventManager.Resource.InfiniteHealthSetted?.Invoke();
        }
        
        public void SetHealthToMax()
        {
            healthValue = maxHealthValue;
        }

        #endregion
    }
}