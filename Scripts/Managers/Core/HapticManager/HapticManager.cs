using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Combat;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Gate;
using _Game.Scripts.Template.GlobalProviders.Interactable.Stacking;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Managers.Core.HapticManager
{
    public class HapticManager : MonoBehaviour
    {
        #region Public Variables

        public SettingsDataSO settingsData;
        
        public float HapticIntensity = 1f;
        public float HapticSharpness = 1f;

        #endregion
        
        #region Unity Lifecycle Methods
        
        private void OnEnable()
        {
            EventManager.InteractableEvents.Interact += OnInteract;
            EventManager.InteractableEvents.GateInteract += OnGateInteract;
            EventManager.ObstacleInteractableEvents.ObstacleInteract += OnObstacleInteract;
            EventManager.StackableEvents.Stack += OnStack;
            EventManager.StackableEvents.Unstack += OnUnstack;
            EventManager.HealthEvents.DamageableDeath += OnDamageableDeath;
            EventManager.InGameEvents.LevelSuccess += OnSuccess;
        }

        private void OnDisable()
        {
            EventManager.InteractableEvents.Interact -= OnInteract;
            EventManager.InteractableEvents.GateInteract -= OnGateInteract;
            EventManager.ObstacleInteractableEvents.ObstacleInteract -= OnObstacleInteract;
            EventManager.StackableEvents.Stack -= OnStack;
            EventManager.StackableEvents.Unstack -= OnUnstack;
            EventManager.HealthEvents.DamageableDeath -= OnDamageableDeath;
            EventManager.InGameEvents.LevelSuccess -= OnSuccess;
        }
        
        #endregion

        #region Public Methods

        public void Vibrate()
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.Haptic(HapticTypes.LightImpact, false, true, this);
        }

        #endregion

        #region Event Callbacks
        private void OnInteract(InteractableData _data)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }

        private void OnGateInteract(GateInteractableData _data)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }

        private void OnObstacleInteract(float value)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }

        private void OnStack(StackableData _data)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }

        private void OnUnstack(StackableData _data)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }

        private void OnDamageableDeath(DamageableData _data)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }
        
        private void OnSuccess()
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
        }
        
        private void OnWeaponSwitch(GameObject weapon)
        {
            if (settingsData.IsVibrationEnabled)
                MMVibrationManager.TransientHaptic(HapticIntensity, HapticSharpness, true, this);
        }
        
        #endregion
    }
}
