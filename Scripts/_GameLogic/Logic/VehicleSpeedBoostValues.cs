using _Game.Scripts.ScriptableObjects.RunTime;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    [CreateAssetMenu(fileName = nameof(VehicleSpeedBoostValues), menuName = "ThisGame/VehicleSpeedBoostValues", order = 0)]
    public class VehicleSpeedBoostValues : PersistentSaveManager<VehicleSpeedBoostValues>, IResettable
    {
        public const float SpeedReduceDuration = 3f;
        
        private bool _isSpeedBoostActive;
        public bool IsSpeedBoostActive => _isSpeedBoostActive;
        
        public float GetSpeedReduceDuration()
        {
            return SpeedReduceDuration;
        }
        
        public void ModifySpeedBoost(bool isActive)
        {
            _isSpeedBoostActive = isActive;
        }
    }
}