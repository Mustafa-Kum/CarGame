using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic;
using _Game.Scripts.General;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace _Game.Scripts.Managers
{
    public class CameraManager : SerializedMonoBehaviour
    {
        #region Variables

        public Dictionary<GameState, CinemachineVirtualCamera> VirtualCameraDictionary = new();
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private float cmBlendTime = 0.5f;
        [SerializeField] private GameObject cameraSpeedBoostEffect;
        [SerializeField] private BoostSpeedDurationUpgradeSO speedBoostDurationUpgradeSO;
        [SerializeField] private PlayerUpgradeData playerUpgradeData;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        #endregion

        #region Event Subscriptions

        private void SubscribeToEvents()
        {
            EventManager.InGameEvents.GameStarted += HandleOnGameStart;
            EventManager.InGameEvents.LevelStart += HandleOnLevelStart;
            EventManager.InGameEvents.LevelSuccess += HandleOnLevelEnd;
            EventManager.InGameEvents.EndMetaStart += HandleOnEndMetaStart;
            EventManager.InGameEvents.LevelLoaded+= HandleOnLevelLoaded;
            EventManager.InGameEvents.PuzzleGameBegin += HandleOnPuzzleGameBegin;
            EventManager.InGameEvents.PuzzleGameEnd += HandleOnPuzzleGameEnd;
            EventManager.InteractableEvents.SlowMotionChaseEffect += HandleOnSlowMotion;
            EventManager.InteractableEvents.SpeedBoostInteract += HandleOnSpeedBoostInteract;
            EventManager.InteractableEvents.CopCrashableObstacleInteract += HandleOnCopVehicleCrash;
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.InGameEvents.GameStarted -= HandleOnGameStart;
            EventManager.InGameEvents.LevelStart -= HandleOnLevelStart;
            EventManager.InGameEvents.LevelSuccess -= HandleOnLevelEnd;
            EventManager.InGameEvents.EndMetaStart -= HandleOnEndMetaStart;
            EventManager.InGameEvents.LevelLoaded -= HandleOnLevelLoaded;
            EventManager.InGameEvents.PuzzleGameBegin -= HandleOnPuzzleGameBegin;
            EventManager.InGameEvents.PuzzleGameEnd -= HandleOnPuzzleGameEnd;
            EventManager.InteractableEvents.SlowMotionChaseEffect -= HandleOnSlowMotion;
            EventManager.InteractableEvents.SpeedBoostInteract -= HandleOnSpeedBoostInteract;
            EventManager.InteractableEvents.CopCrashableObstacleInteract -= HandleOnCopVehicleCrash;
        }

        #endregion

        #region Event Handlers

        private void HandleOnGameStart()
        {
            SwitchVirtualCamera(GameState.LevelLoaded);
        }
        
        private void HandleOnLevelStart()
        {
            DOVirtual.DelayedCall( 1f, () =>
            {
                SwitchVirtualCamera(GameState.LevelStart);
            });
        }
        private void HandleOnLevelLoaded(GameObject arg0)
        {
            HandleOnGameStart();
        }
        
        private void HandleOnLevelEnd()
        {
            SwitchVirtualCamera(GameState.LevelEnd);
        }

        private void HandleOnEndMetaStart()
        {
            SwitchVirtualCamera(GameState.EndMetaStart);
        }
        
        private void HandleOnLevelFail()
        {
            SwitchVirtualCamera(GameState.Fail);
        }
        
        private void SetBlendTime(float blendTime)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = blendTime;
        }
        
        private void HandleOnSlowMotion()
        {
            SetBlendTime(cmBlendTime);
            
            SwitchVirtualCamera(GameState.SlowMotion);
            
            DOVirtual.DelayedCall(3,  () =>
            {
                SetBlendTime(1f);
                SwitchVirtualCamera(GameState.LevelStart);
            });
        }
        
        private void HandleOnPuzzleGameBegin()
        {
            SwitchVirtualCamera(GameState.PuzzleCamera);
        }
        
        private void HandleOnPuzzleGameEnd()
        {
            DOVirtual.DelayedCall(6f, () => { SwitchVirtualCamera(GameState.GarageCamera); });
        }

        #endregion

        #region Camera Management

        private void SwitchVirtualCamera(GameState state)
        {
            DisableAllCameras();

            if (VirtualCameraDictionary.ContainsKey(state))
            {
                VirtualCameraDictionary[state].gameObject.SetActive(true);
            }
        }

        private void DisableAllCameras()
        {
            foreach (var cam in VirtualCameraDictionary.Values)
            {
                cam.gameObject.SetActive(false);
            }
        }
        
        private void HandleOnSpeedBoostInteract()
        {
            ActivateCameraSpeedBoostEffect(true);
            IncreaseFOVChaseCamera(speedBoostDurationUpgradeSO.GetValue(playerUpgradeData.GetUpgradeLevel(UpgradeType.SpeedBoostDuration)));
        }
        
        private void HandleOnCopVehicleCrash()
        {
            SetBlendTime(0);
            
            SwitchVirtualCamera(GameState.PoliceVehicleCrash);
            
            DOVirtual.DelayedCall(2, () =>
            {
                SwitchVirtualCamera(GameState.LevelStart);
                SetBlendTime(1f);
            });
        }

        private CinemachineVirtualCamera GetCameraFromState(GameState state)
        {
            return VirtualCameraDictionary[state];
        }
        
        private void IncreaseFOVChaseCamera(float duration)
        {
            CinemachineVirtualCamera chaseCamera = GetCameraFromState(GameState.LevelStart);
            
            DOTween.To(() => chaseCamera.m_Lens.FieldOfView, x => chaseCamera.m_Lens.FieldOfView = x,
                chaseCamera.m_Lens.FieldOfView + 20, 0.2f);
            
            DOVirtual.DelayedCall(duration, () =>
            {
                DOTween.To(() => chaseCamera.m_Lens.FieldOfView, x => chaseCamera.m_Lens.FieldOfView = x,
                    chaseCamera.m_Lens.FieldOfView - 20, 0.2f);
                
                ActivateCameraSpeedBoostEffect(false);
            });
        }
        
        private void ActivateCameraSpeedBoostEffect(bool isEnabled)
        {
            cameraSpeedBoostEffect.SetActive(isEnabled);
        }

        #endregion
    }
}
