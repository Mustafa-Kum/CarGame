﻿using System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    public class ResetManager : MonoBehaviour
    {
        #region Inspector Variables

        public ResettableData resettableData;

        #endregion

        #region Event Methods

        private void Awake()
        {
            EventManager.InGameEvents.LevelStart += SaveInitialData;
            EventManager.InGameEvents.LevelFail += Reset;
            EventManager.InGameEvents.LevelSuccess += Reset;
            EventManager.InGameEvents.LevelRestart += Reset;
            
            Reset();
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= SaveInitialData;
            EventManager.InGameEvents.LevelFail -= Reset;
            EventManager.InGameEvents.LevelSuccess -= Reset;
            EventManager.InGameEvents.LevelRestart -= Reset;
        }

        #endregion

        #region Private Methods

        private void SaveInitialData()
        {
            foreach (var resettable in resettableData.resettableData)
            {
                resettable.I.SaveInitialState();
            }
        }
        
        private void Reset()
        {
            resettableData.ResetAllData();
        }
        

        #endregion
    }
}