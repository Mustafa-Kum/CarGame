using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = "CarMeshIndex", menuName = "ThisGame/Player/CarMeshIndex", order = 1)]
    public class CarMeshIndexSO : PersistentSaveManager<PlayerSaveableData>, IResettable
    {
        #region PrivateFields
        
        [SerializeField] private int _carMeshIndex;

        #endregion

        #region PublicProperties

        public int CarMeshIndex { get => _carMeshIndex; set => _carMeshIndex = value; }

        #endregion
        
        #region PublicMethods
        
        public void IncreaseCarMeshIndex()
        {
            _carMeshIndex++;
        }
        
        public void Reset()
        {
            _carMeshIndex = 0;
        }
        
        #endregion
    }
}