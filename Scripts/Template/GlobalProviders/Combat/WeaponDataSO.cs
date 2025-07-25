using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ThisGame/WeaponData", order = 0)]
    public class WeaponDataSO : UpgradableSO
    {
        #region Public Variables
        
        public float a_fireRate = 0.01f, b_fireRate = 0.2f, c_fireRate = 2f;

        #endregion
        
        #region Public Methods

        public float GetFireRate(int currentLevel)
        {
            return a_fireRate * Mathf.Pow(currentLevel,2)+b_fireRate*currentLevel+c_fireRate;
        }

        #endregion
    }
}