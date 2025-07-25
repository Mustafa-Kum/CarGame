using System;
using UnityEngine;
using _Game.Scripts._GameLogic.Data.Visual;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;

namespace _Game.Scripts._GameLogic.Logic.Manager
{
    public class GridManager : MonoBehaviour
    {
        #region Public Variables

        public ParticleContainer particleContainer;
        public GridConfig gridConfig;

        #endregion

        #region Properties

        public ParticleContainer ParticleContainer => particleContainer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            EventManager.GridEvents.OnGridConfigCreated?.Invoke(gridConfig.GetGridConfig());   
        }

        #endregion

 
    }
}

[Serializable]
public enum GridObjectType
{
    Red,
    Green,
    Blue,
    Yellow,
    Matched,
    Obstacle,
    None,
    Bomb,
    WallCoin
}
