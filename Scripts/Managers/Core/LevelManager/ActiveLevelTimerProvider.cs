using _Game.Scripts.Helper.Extensions.System;
using Handler.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;

namespace _Game.Scripts.Managers.Core
{
    public class ActiveLevelTimerProvider : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private LevelTimerValuesSO levelTimerValuesSO;  // Reference to your ScriptableObject

        #endregion

        #region Private Variables

        private float startTime;
        private float endTime;
        private bool isTimerActive;

        #endregion

        #region Unity Lifecycle Methods

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += StartTimer;
            EventManager.InGameEvents.LevelSuccess += StopTimer;
            EventManager.InGameEvents.LevelFail += StopTimer;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= StartTimer;
            EventManager.InGameEvents.LevelSuccess -= StopTimer;
            EventManager.InGameEvents.LevelFail -= StopTimer;
        }

        #endregion

        #region Timer Methods

        private void StartTimer()
        {
            isTimerActive = true;
            startTime = Time.time;

            EventManager.TimerEvents.LevelTimerBegin?.Invoke();
            TDebug.Log($"Level timer started. Start time: {startTime}");
        }

        private void StopTimer()
        {
            if (isTimerActive)
            {
                isTimerActive = false;
                endTime = Time.time;

                float elapsedTime = endTime - startTime;
                levelTimerValuesSO.AddTime(elapsedTime);  // Directly use SO to save the time

                EventManager.TimerEvents.LevelTimerEnd?.Invoke(elapsedTime);
                TDebug.Log($"Level timer ended. Elapsed time: {elapsedTime}");
                TDebug.Log($"Elapsed times stored: {string.Join(", ", levelTimerValuesSO.sessionTimes)}");
            }
        }

        #endregion
    }
}