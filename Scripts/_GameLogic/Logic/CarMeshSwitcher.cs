using System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class CarMeshSwitcher : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Mesh[] _meshes;
        [SerializeField] private Material[] _materials;
        [SerializeField] private CarMeshIndexSO _carMeshIndexSO;
        [SerializeField] private PlayerSavableData _playerSavableData;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                MeshSwitcherWithKeybord();
            }
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.AfterLevelSuccess += IncreaseMeshLevelIndex;
            EventManager.InGameEvents.LevelLoaded += ChangeMeshWithLevelIndex;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.AfterLevelSuccess -= IncreaseMeshLevelIndex;
            EventManager.InGameEvents.LevelLoaded -= ChangeMeshWithLevelIndex;
        }

        private void ChangeMeshWithLevelIndex(GameObject levelGo)
        {
            MeshSwitcher();
        }
        
        [Button]
        private void MeshSwitcher()
        {
            _carMeshIndexSO.CarMeshIndex = _playerSavableData.LevelIndex;
            
            if (_carMeshIndexSO.CarMeshIndex >= _meshes.Length)
            {
                _carMeshIndexSO.CarMeshIndex = 0;
            }
            
            _meshFilter.mesh = _meshes[_carMeshIndexSO.CarMeshIndex];
            
            _meshRenderer.material = _materials[_carMeshIndexSO.CarMeshIndex];
        }
        
        private void MeshSwitcherWithKeybord()
        {
            _carMeshIndexSO.CarMeshIndex++;
            
            if (_carMeshIndexSO.CarMeshIndex >= _meshes.Length)
            {
                _carMeshIndexSO.CarMeshIndex = 0;
            }
            
            _meshFilter.mesh = _meshes[_carMeshIndexSO.CarMeshIndex];
            
            _meshRenderer.material = _materials[_carMeshIndexSO.CarMeshIndex];
        }
        
        private void IncreaseMeshLevelIndex()
        {
            _carMeshIndexSO.IncreaseCarMeshIndex();
        }
    }
}