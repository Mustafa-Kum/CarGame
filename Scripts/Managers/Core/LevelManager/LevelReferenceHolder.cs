using Dreamteck.Splines;
using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Scripts.InGame.ReferenceHolder
{
    public class LevelReferenceHolder : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private Transform _successTrigger;
        
        [SerializeField] private Transform _charSpawnPoint;
        
        [SerializeField] private Transform _puzzleCarTransform;
        [SerializeField] private Transform _garageTransform;
        [SerializeField] private GameObject _carHight;

        #endregion

        #region Properties

        public Transform SuccessTrigger => _successTrigger;
        public Transform CharSpawnPoint => _charSpawnPoint;
        
        public Transform PuzzleCarTransform => _puzzleCarTransform;
        
        public Transform GarageTransform => _garageTransform;
        
        public GameObject CarHight => _carHight;

        #endregion
    }
}
