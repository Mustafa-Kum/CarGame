using System;
using _Game.Scripts.ScriptableObjects.Saveable;
using Fluxy;
using Sirenix.Serialization;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    public abstract class ShooterProvider : MonoBehaviour
    {
        #region Public Variables

        [OdinSerialize] public UpgradableSO fireRateUpgradable;
        [OdinSerialize] public UpgradableSO _speedUpgradable;
        [OdinSerialize] public UpgradableSO _rangeUpgradable;
        [OdinSerialize] public UpgradableSO _damageUpgradable;
        
        #endregion
        
        private void GetFireRateData()
        {

        }
    }
    
    [Serializable]
    public struct ProjectileStructData
    {
        public float speed;
        public float range;
        public float damage;
    }
    
    [Serializable]
    public struct WeaponStructData
    {
        internal float fireRate;
        public GameObject weaponObject;
        public GameObject bulletObject;
        public Transform muzzle;
        public FluxyTarget target;
    }
}