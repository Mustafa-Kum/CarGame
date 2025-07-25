using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class ActiveGameObjectOnPathFound : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObjectToActivate;
        
        private void OnEnable()
        {
            EventManager.GridEvents.OnPathFound += OnPathFound;
        }
        
        private void OnDisable()
        {
            EventManager.GridEvents.OnPathFound -= OnPathFound;
        }

        private void OnPathFound(List<GridTile> arg0)
        {
            if (_gameObjectToActivate != null)
                _gameObjectToActivate.SetActive(true);
        }
    }
}