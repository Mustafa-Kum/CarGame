using System;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using _Game.Scripts.UI.Buttons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core
{
    public static partial class EventManager
    {
        public static class InGameEvents
        {
            public static UnityAction GameStarted;
            public static UnityAction LoadLevel;
            public static UnityAction BeforeLevelLoaded;
            public static UnityAction<GameObject> LevelLoaded;
            public static UnityAction LevelStart;
            public static UnityAction LevelSuccess;
            public static UnityAction AfterLevelSuccess;
            public static UnityAction EndMetaStart;
            public static UnityAction LevelRestart;
            public static UnityAction LevelFail;
            public static UnityAction LevelFinish;
            public static UnityAction PuzzleGameTransformTrigger;
            public static UnityAction PuzzleGameBegin;
            public static UnityAction PuzzleTableAscend;
            public static UnityAction PuzzleGameEnd;
            public static UnityAction GarageAction;
        }
        
        public static class SaveEvents
        {
            public static UnityAction DataSaved;
            public static UnityAction DataLoaded;
        }
        
        public static class AudioEvents
        {
            public static UnityAction<int, AudioClip> AudioAdded;
            public static UnityAction AudioStop;
            public static UnityAction<SoundType,bool,bool> AudioPlay;
            public static UnityAction<float> VolumeChange;
            public static UnityAction<int, float> AudioChanged;
            public static UnityAction<bool> AudioLoopToggleChanged;
            public static UnityAction<bool> AudioEnabled;
        }
        
        public static class AdEvents
        {
            public static UnityAction<Action> ShowRewarded;
            public static UnityAction ShowInterstitial;
            public static UnityAction<Action> ShowRewardedInterstitial;
            public static UnityAction ShowBanner;
        }
        
        public static class VehicleEvents
        {
            public static UnityAction<Transform> VehicleSpawned;
        }
        
        public static class AnalyticsEvents
        {
            public static UnityAction AnalyticsInitialized;
        }
    }
}