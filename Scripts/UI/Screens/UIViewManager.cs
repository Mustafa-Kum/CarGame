using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.UI.Screens
{
    public class UIViewManager : SerializedMonoBehaviour
    {
        #region Variables

        [SerializeField] private Dictionary<GameState, GameObject> _gameStateDictionary;

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
            EventManager.InGameEvents.LevelSuccess += HandleOnLevelEnd;
            EventManager.InGameEvents.LevelStart += HandleOnLevelStart; 
            EventManager.InGameEvents.LoadLevel += HandleOnGameStart;
            EventManager.InGameEvents.LevelFail += HandleOnLevelFail;
            EventManager.InGameEvents.PuzzleGameEnd += HandleOnPuzzleGameEnd;
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.InGameEvents.GameStarted -= HandleOnGameStart;
            EventManager.InGameEvents.LevelSuccess -= HandleOnLevelEnd;
            EventManager.InGameEvents.LevelStart -= HandleOnLevelStart;
            EventManager.InGameEvents.LoadLevel -= HandleOnGameStart;
            EventManager.InGameEvents.LevelFail -= HandleOnLevelFail;
            EventManager.InGameEvents.PuzzleGameEnd -= HandleOnPuzzleGameEnd;
        }

        #endregion

        #region Event Handlers

        private void HandleOnGameStart()
        {
            OpenPanel(GameState.LevelLoaded);
        }

        private void HandleOnLevelStart()
        {
            OpenPanel(GameState.LevelStart);
        }
        
        private void HandleOnLevelEnd()
        {
            OpenPanel(GameState.LevelEnd);
        }
        
        private void HandleOnLevelFail()
        {
            OpenPanel(GameState.Fail);
        }
        
        private void HandleOnPuzzleGameEnd()
        {
            DOVirtual.DelayedCall(13f, () =>
            {
                OpenPanel(GameState.Shop);
                EventManager.InGameEvents.LevelFinish?.Invoke();
            });
        }

        #endregion

        #region Panel Management

        private void OpenPanel(GameState state)
        {
            CloseAllPanels();
            if (_gameStateDictionary.ContainsKey(state))
            {
                _gameStateDictionary[state].SetActive(true);
            }
        }

        private void CloseAllPanels()
        {
            foreach (var panel in _gameStateDictionary.Values)
            {
                panel.SetActive(false);
            }
        }

        #endregion
    }
}
