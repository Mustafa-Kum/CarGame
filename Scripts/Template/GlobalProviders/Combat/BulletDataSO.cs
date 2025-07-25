using _Game.Scripts.ScriptableObjects.RunTime;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "ThisGame/BulletData", order = 0)]
    public class BulletDataSO : UpgradableSO
    {
        #region Public Variables
        
        public float a_damage = 0.05f, b_damage = 1f, c_damage = 10f;
        public float a_range = 0.04f, b_range = 0.8f, c_range = 8f;
        public float a_speed = 0.01f, b_speed = 0.2f, c_speed = 2f;

        #endregion
        
        public float GetDamage(int currentLevel)
        {
            return a_damage * Mathf.Pow(currentLevel,2)+b_damage*currentLevel+c_damage;
        }
        
        public float GetRange(int currentLevel)
        {
            return a_range * Mathf.Pow(currentLevel,2)+b_range*currentLevel+c_range;
        }
        
        public float GetSpeed(int currentLevel)
        {
            return a_speed * Mathf.Pow(currentLevel,2)+b_speed*currentLevel+c_speed;
        }
    }
}