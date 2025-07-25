using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Manager.Visual.Logic
{
    public class PlayParticleOnPathFound : MonoBehaviour
    {
        #region INPSECTOR VARIABLES
        
        [SerializeField] private ParticleSystem _particleSystem;
        
        #endregion
        
        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.GridEvents.OnPathFound += PlayParticle;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnPathFound -= PlayParticle;
        }
        
        #endregion

        #region PRIVATE METHODS

        private void PlayParticle(List<GridTile> arg0)
        {
            if (_particleSystem != null) _particleSystem.Play();
        }


        #endregion
    }
}