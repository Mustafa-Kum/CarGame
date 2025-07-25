using UnityEngine;
using _Game.Scripts.General;
using _Game.Scripts.Managers.Core;

namespace _Game.Scripts.Managers
{
    public class GameStateManager : MonoBehaviour
    {
        #region Unity Lifecycle Methods
        
        private void Awake() => SubscribeEvents();

        private void OnDisable() => UnsubscribeEvents();

        #endregion

        #region Event Subscription Methods

        private void SubscribeEvents()
        {
            EventManager.InGameEvents.LevelLoaded += OnLevelLoaded;
            EventManager.InGameEvents.LevelStart += OnLevelStart;
            EventManager.InGameEvents.LevelSuccess += OnLevelSuccess;
            EventManager.InGameEvents.LevelFail += OnLevelFail;
            EventManager.InGameEvents.EndMetaStart += OnEndMetaStart;
        }

        private void UnsubscribeEvents()
        {
            EventManager.InGameEvents.LevelLoaded -= OnLevelLoaded;
            EventManager.InGameEvents.LevelStart -= OnLevelStart;
            EventManager.InGameEvents.LevelSuccess -= OnLevelSuccess;
            EventManager.InGameEvents.LevelFail -= OnLevelFail;
            EventManager.InGameEvents.EndMetaStart -= OnEndMetaStart;
        }

        #endregion

        #region Event Handlers

        // Changes the game state when the level is loaded
        private void OnLevelLoaded(GameObject go)
        {
            GameStateData.ChangeGameState(GameState.LevelLoaded);
        }

        // Changes the game state when the level starts
        private void OnLevelStart()
        {
            GameStateData.ChangeGameState(GameState.LevelStart);
        }

        // Changes the game state when the level ends in success
        private void OnLevelSuccess()
        {
            GameStateData.ChangeGameState(GameState.Success);
        }

        // Changes the game state when the level ends in failure
        private void OnLevelFail()
        {
            GameStateData.ChangeGameState(GameState.Fail);
        }

        // Changes the game state when the meta end starts
        private void OnEndMetaStart()
        {
            GameStateData.ChangeGameState(GameState.EndMetaStart);
        }

        #endregion
    }
}
