using _Game.Scripts.Managers.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class SlowMotionTimeSettingsManager : SerializedMonoBehaviour
    {
        [SerializeField] private float desiredSlowMotionFactor = 0.1f;

        private void OnEnable()
        {
            EventManager.InteractableEvents.SlowMotionChaseEffect += SetSlowMotionSettings;
            EventManager.InteractableEvents.CopCrashableObstacleInteract += SetSlowMotionSettings;
        }

        private void OnDisable()
        {
            EventManager.InteractableEvents.SlowMotionChaseEffect -= SetSlowMotionSettings;
            EventManager.InteractableEvents.CopCrashableObstacleInteract -= SetSlowMotionSettings;
        }

        private void SetSlowMotionSettings()
        {
            Time.timeScale = desiredSlowMotionFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            
            DOVirtual.DelayedCall(4, () =>
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            });
        }
    }
}