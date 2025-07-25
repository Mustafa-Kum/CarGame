using _Game.Scripts.ScriptableObjects.Predefined;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class LevelConfigurationsProvider
    {
        #region Public Variables

        private readonly InLevelObjectsConfigurationsData _inLevelObjectsConfigurationsData;
        private readonly Transform _levelHolder;

        #endregion

        #region Private Variables

        private GameObject _instantiatedBackground;
        private GameObject _instantiatedPathFollower;
        private GameObject _instantiatedParticle;

        #endregion

        #region Constructors

        public LevelConfigurationsProvider(InLevelObjectsConfigurationsData inLevelObjectsConfigurationsData, Transform levelHolder)
        {
            _inLevelObjectsConfigurationsData = inLevelObjectsConfigurationsData;
            _levelHolder = levelHolder;
        }
        
        #endregion

        #region Public Methods

        public void InitializeLevelConfigurations()
        {
            InitializeSkyboxMaterial();
        }

        #endregion
        

        #region Private Methods

       private void InitializeSkyboxMaterial()
        {
            if (_inLevelObjectsConfigurationsData.SkyboxMaterial != null)
            {
                RenderSettings.skybox = _inLevelObjectsConfigurationsData.SkyboxMaterial;
            }
        }

        #endregion
    }
}