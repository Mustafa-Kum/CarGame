using System;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = "Level_SO", menuName = "ThisGame/Levels/LevelSO", order = 2)]
    public class Level_SO : ScriptableObject
    {
        #region Public Variables

        [SerializeField] private GameObject _levelPrefab;
        public InLevelObjectsConfigurationsData inLevelObjectsConfigurationsData;
        public bool isTutorialLevel;
        
        #endregion
        
        #region Properties

        public GameObject LevelPrefab => _levelPrefab;

        #endregion
    }

    [Serializable]
    public class InLevelObjectsConfigurationsData
    {
        [SerializeField] private Material _skyboxMaterial;

        #region Properties
        
        public Material SkyboxMaterial => _skyboxMaterial;
        
        #endregion
    }
}
