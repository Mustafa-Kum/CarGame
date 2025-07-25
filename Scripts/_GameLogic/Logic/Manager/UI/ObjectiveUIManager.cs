using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.ObjectiveProgression;
using _Game.Scripts._GameLogic.Logic.Manager.Objective;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.UI
{
    public class ObjectiveUIManager : SerializedMonoBehaviour
    {
        #region PUBLIC VARIABLES
        
        public LevelProgressionData LevelProgressionData;
        public PlayerSavableData PlayerSavableData;
        
        [FoldoutGroup(nameof(ObjectiveUIDictionary))]
        public Dictionary<GridObjectType, ObjectiveUIElement> ObjectiveUIDictionary;

        #endregion

        #region PRIVATE VARIABLES

        private Dictionary<GridObjectType, int> _objectiveDictionary;

        #endregion

        #region UNITY METHODS
        
        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += OnLevelStarted;
            EventManager.ObjectiveEvents.ObjectiveUpdated += OnObjectiveUpdated;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= OnLevelStarted;
            EventManager.ObjectiveEvents.ObjectiveUpdated -= OnObjectiveUpdated;
        }

        #endregion
        
        #region PRIVATE METHODS
        
        private void OnLevelStarted()
        {
            if (ObjectiveUIDictionary == null || ObjectiveUIDictionary.Count == 0) return;
            
            LevelProgressionItems levelObjectives = LevelProgressionData.GetLevelObjectives(PlayerSavableData.LevelIndex);
    
            _objectiveDictionary = new Dictionary<GridObjectType, int>();
            HashSet<GridObjectType> activeTypes = new HashSet<GridObjectType>();

            foreach (var objective in levelObjectives.Objectives)
            {
                _objectiveDictionary.Add(objective.Type, objective.RequiredCount);
                activeTypes.Add(objective.Type);

                if (ObjectiveUIDictionary.TryGetValue(objective.Type, out ObjectiveUIElement uiElement))
                {
//                    uiElement.TextElement.text = objective.RequiredCount.ToString();
                }
            }

            ActivateObjectives(activeTypes);
        }

        private void ActivateObjectives(HashSet<GridObjectType> activeTypes)
        {
            foreach (var entry in ObjectiveUIDictionary)
            {
                entry.Value.TransformElement.gameObject.SetActive(activeTypes.Contains(entry.Key));
            }
        }
        
        private void OnObjectiveUpdated(GridObjectType type, int count)
        {
            if (!_objectiveDictionary.ContainsKey(type)) return;

            _objectiveDictionary[type] = count;

            if (ObjectiveUIDictionary.TryGetValue(type, out ObjectiveUIElement uiElement))
            {
                uiElement.ParticleSystem.Stop();
                uiElement.ParticleSystem.Play();
                
                uiElement.TextElement.text = count.ToString();
            }
        }

        #endregion
    }
}


[Serializable] 
public class ObjectiveUIElement
{
    public TextMeshProUGUI TextElement;
    public Transform TransformElement;
    public ParticleSystem ParticleSystem;
}
