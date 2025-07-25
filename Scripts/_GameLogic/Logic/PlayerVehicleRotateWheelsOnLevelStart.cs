using System;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class PlayerVehicleRotateWheelsOnLevelStart : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation[] animations;

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += PlayTweens;
            EventManager.InGameEvents.LevelFinish += StopWheels;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= PlayTweens;
            EventManager.InGameEvents.LevelFinish -= StopWheels;
        }
        
        private void PlayTweens()
        {
            foreach (var animation in animations)
            {
                animation.DORestart();
            }
        }
        
        public void StopWheels()
        {
            foreach (var animation in animations)
            {
                animation.DOPause();
            }
        }
    }
}