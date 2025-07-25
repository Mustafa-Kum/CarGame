﻿using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.RunTime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "LevelTimerValues", menuName = "ThisGame/LevelTimerValues", order = 0)]
    public class LevelTimerValuesSO : PersistentSaveManager<LevelTimerValuesSO>, IResettable
    {
        #region Public Variables

        [ListDrawerSettings(CustomAddFunction = "CreateNewSessionData", OnTitleBarGUI = "DrawTitleBarElements")]
        public List<SessionData> sessionTimes = new List<SessionData>();

        #endregion

        #region Public Methods

        public void AddTime(float time)
        {
            var newSession = CreateNewSessionData();
            newSession.Time = time;
            sessionTimes.Add(newSession);
        }
        
        public void RemoveTimeFromListIndex(int index)
        {
            sessionTimes.RemoveAt(index);
        }

        #endregion

        #region Odin Inspector Functions

        private SessionData CreateNewSessionData()
        {
            return new SessionData { Index = sessionTimes.Count };
        }

        private void DrawTitleBarElements()
        {
            if (GUILayout.Button("Reset All", GUILayout.Width(80)))
            {
                sessionTimes.Clear();
            }
        }

        #endregion
    }
    
    [System.Serializable]
    public class SessionData
    {
        public float Time;

        [ShowInInspector, DisplayAsString, HideLabel, DisableInEditorMode]
        public string SessionLabel => $"Session {Index + 1}";

        [HideInInspector]
        public int Index;
    }
}