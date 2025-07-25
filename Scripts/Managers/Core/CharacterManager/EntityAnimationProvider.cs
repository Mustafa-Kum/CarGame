using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.CharacterManager
{
    public class EntityAnimationProvider : SerializedMonoBehaviour
    {
        #region Public Variables

        public Animator animator;

        #endregion

        #region Private Variables

        public Dictionary<GameState, string> gameStateAnimationData;

        #region Animator Hashes

        private int Idle;
        private int Running;
        private int Fail;
        private int Success;

        #endregion

        #endregion

        #region Unity Methods

        private void Awake()
        {
            AnimationDictHashing();
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelLoaded += HandleLevelLoaded;
            EventManager.InGameEvents.LevelStart += HandleLevelStart;
            EventManager.InGameEvents.LevelFail += HandleLevelFail;
            EventManager.InGameEvents.LevelSuccess += HandleLevelSuccess;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelLoaded -= HandleLevelLoaded;
            EventManager.InGameEvents.LevelStart -= HandleLevelStart;
            EventManager.InGameEvents.LevelFail -= HandleLevelFail;
            EventManager.InGameEvents.LevelSuccess -= HandleLevelSuccess;
        }

        #endregion

        #region Private Methods

        private void AnimationDictHashing()
        {
            Idle = Animator.StringToHash(gameStateAnimationData[GameState.LevelLoaded]);
            Running = Animator.StringToHash(gameStateAnimationData[GameState.LevelStart]);
            Fail = Animator.StringToHash(gameStateAnimationData[GameState.Fail]);
            Success = Animator.StringToHash(gameStateAnimationData[GameState.Success]);
        }

        private void HandleLevelLoaded(GameObject go)
        {
            SwitchAnimation(GameState.LevelLoaded);
        }
        
        private void HandleLevelStart()
        {
            SwitchAnimation(GameState.LevelStart);
        }
        
        private void HandleLevelSuccess()
        {
            SwitchAnimation(GameState.Success);
        }

        private void HandleLevelFail()
        {
            SwitchAnimation(GameState.Fail);
        }

        private void SwitchAnimation(GameState state)
        {
            if (gameStateAnimationData.TryGetValue(state, out var value))
            {
                animator.SetTrigger(Animator.StringToHash(value));
            }
        }

        #endregion
    }
}
