using System;
using System.Collections.Generic;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using AlmostEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Tutorial.Logic
{
    [Serializable]
    public class TutorialStepByLevel
    {
        public int levelIndex;
        public InterfaceSerialization<ITutorialStep>[] steps;
        public bool isTutorialCompleted;
    }

    public class TutorialManager : Singleton<TutorialManager>
    {
        #region INSPECTOR VARIABLES

        public PlayerSavableData playerSavableData;

        [SerializeField] public TutorialStepByLevel[] stepsByLevel;

        public List<Button> boosterButtons;

        // private readonly Dictionary<BoosterType, int> _boosterTypeToTutorialIndexMapping = new()
        // {
        //     { BoosterType.RowChanger, 0 },
        //     { BoosterType.GridTileMatcher, 1 }
        // };

        private readonly Dictionary<int, float> _levelStartDelays = new()
        {
            {
                //Key: Level Index, Value: Tutorial Start Delay
                0, 1f
            }
        };

        #endregion

        #region PRIVATE VARIABLES

        private int _currentStepIndex;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += ClearBoosterButtonListCache;
            EventManager.InGameEvents.LevelSuccess += ResetCurrentStepIndex;
            EventManager.InGameEvents.LevelFail += ResetCurrentStepIndex;
            EventManager.InGameEvents.LevelFail += ClearBoosterButtonListCache;
           // EventManager.UIEvents.OnBoosterButtonExistOnUI += AddBoosterButtonSorted;
            EventManager.TutorialEvents.LevelStartTutorialCaching += HandleLevelBasedDelayValueAdvancing;
            EventManager.InGameEvents.PuzzleTableAscend += CallTutorialAdvancingWithDelay;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= ClearBoosterButtonListCache;
            EventManager.InGameEvents.LevelSuccess -= ResetCurrentStepIndex;
            EventManager.InGameEvents.LevelFail -= ResetCurrentStepIndex;
            EventManager.InGameEvents.LevelFail -= ClearBoosterButtonListCache;
            //EventManager.UIEvents.OnBoosterButtonExistOnUI -= AddBoosterButtonSorted;
            EventManager.TutorialEvents.LevelStartTutorialCaching -= HandleLevelBasedDelayValueAdvancing;
            EventManager.InGameEvents.PuzzleTableAscend -= CallTutorialAdvancingWithDelay;
        }

        #endregion

        #region PUBLIC METHODS

        private void CallTutorialAdvancingWithDelay()
        {
            DOVirtual.DelayedCall(5, () => AdvanceDelayedCurrentStep(0f));
        }
        
        [Button]
        public void AdvanceDelayedCurrentStep(float endDelay)
        {
            if (playerSavableData.IsAllTutorialCompleted) return;

            var currentLevelSteps = Array.Find(stepsByLevel, level => level.levelIndex == playerSavableData.LevelIndex);
            if (currentLevelSteps == null) return;

            if (currentLevelSteps.isTutorialCompleted) return;

            if (_currentStepIndex >= currentLevelSteps.steps.Length)
            {
                currentLevelSteps.isTutorialCompleted = true;
                DelayedDisableAllSteps(endDelay);
                CheckIfAllTutorialsCompleted();
                return;
            }

            DisablePreviousStep(currentLevelSteps);
            currentLevelSteps.steps[_currentStepIndex].I.ActivateStep();
            _currentStepIndex++;
        }

        public void DelayedAdvanceCurrentStep(float delay) => DOVirtual.DelayedCall(delay, () => AdvanceDelayedCurrentStep(0));

        [Button]
        public void ResetTutorial()
        {
            foreach (var level in stepsByLevel)
            {
                level.isTutorialCompleted = false;
                foreach (var step in level.steps) step.I.DisableStep();
            }

            _currentStepIndex = 0;
        }

        public void ClickBoosterButtonFromBoosterButtonIndex(int index) => boosterButtons[index].onClick.Invoke();

        #endregion

        #region PRIVATE METHODS

        /*private void AddBoosterButtonSorted(BoosterType boosterType, Button button)
        {
            if (boosterButtons.Contains(button)) return;
            if (_boosterTypeToTutorialIndexMapping.TryGetValue(boosterType, out var tutorialIndex))
            {
                if (boosterButtons.Count > tutorialIndex)
                {
                    boosterButtons[tutorialIndex] = button;
                }
                else
                {
                    while (boosterButtons.Count <= tutorialIndex) boosterButtons.Add(null);
                    boosterButtons[tutorialIndex] = button;
                }
            }
            else
            {
                boosterButtons.Add(button);
            }
        }*/

        private float GetDelayForLevel(int levelIndex) => _levelStartDelays.GetValueOrDefault(levelIndex, 0f);

        private void HandleLevelBasedDelayValueAdvancing(float endDelay) => DOVirtual.DelayedCall(GetDelayForLevel(playerSavableData.LevelIndex), () => AdvanceDelayedCurrentStep(endDelay));

        private void DisablePreviousStep(TutorialStepByLevel currentLevelSteps)
        {
            if (_currentStepIndex > 0) currentLevelSteps.steps[_currentStepIndex - 1].I.DisableStep();
        }

        private void DelayedDisableAllSteps(float delay) => DOVirtual.DelayedCall(delay, DisableAllSteps);

        private void ClearBoosterButtonListCache() => boosterButtons.Clear();

        private void DisableAllSteps()
        {
            foreach (var level in stepsByLevel)
            foreach (var step in level.steps)
                step.I.DisableStep();
        }

        private void ResetCurrentStepIndex() => _currentStepIndex = 0;

        private void CheckIfAllTutorialsCompleted()
        {
            if (!Array.TrueForAll(stepsByLevel, level => level.isTutorialCompleted)) return;
            playerSavableData.IsAllTutorialCompleted = true;
            EventManager.TutorialEvents.TutorialCompleted?.Invoke();
        }

        #endregion
    }
}