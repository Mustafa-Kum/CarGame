using _Game.Scripts._GameLogic.Data.ObjectiveProgression;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Objective
{
    public class LevelProgressionManager : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        [SerializeField] private LevelProgressionData _levelProgressionData;
        
        [SerializeField] private PlayerSavableData _playerSavableData;

        [SerializeField] private LevelProgressionItems _currentLevelObjectives;
        
        #endregion

        #region PRIVATE VARIABLES


        #endregion

        #region UNITY METHODS

        private void Awake()
        {
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += OnLevelStarted;
            EventManager.GridEvents.GridObjectMatchedType += OnGridObjectMatchedType;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= OnLevelStarted;
            EventManager.GridEvents.GridObjectMatchedType -= OnGridObjectMatchedType;
        }

        #endregion
        
        #region PRIVATE METHODS
        
        private void OnLevelStarted()
        {
            LevelProgressionItems levelObjectives = _levelProgressionData.GetLevelObjectives(_playerSavableData.LevelIndex);
            _currentLevelObjectives = new LevelProgressionItems(levelObjectives.Objectives);
            
            EventManager.ObjectiveEvents.LevelProgressionItemsUpdated.Invoke(_currentLevelObjectives.Objectives);
        }

        private void OnGridObjectMatchedType(GridObjectType type, Vector3 position)
        {
           EventManager.ObjectiveEvents.LevelProgressionItemsUpdated.Invoke(_currentLevelObjectives.Objectives);

            DOVirtual.DelayedCall(0.5f, () => CheckObjectivesAndTriggerLevelSuccess(type));
        }

        private void CheckObjectivesAndTriggerLevelSuccess(GridObjectType type)
        {
            _currentLevelObjectives.ReduceObjective(type);
            if (_currentLevelObjectives.IsObjectivesCompleted())
            {
                Debug.Log("Level Success");
            }
        }
        
        #endregion
    }
}