using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Base
{
    public abstract class BaseObjectVisualManager : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        public LevelList_SO _levelListSO;
        
        public Transform _parentTransform;
        
        public GameObject _currentObstacle;
        
        #endregion

        #region PUBLIC METHODS

        public abstract void PerformChangeObjectVisual();

        #endregion
    }
}