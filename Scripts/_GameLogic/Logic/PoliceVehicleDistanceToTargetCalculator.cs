using System;
using System.Globalization;
using _Game.Scripts.Managers.Core;
using TMPro;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class PoliceVehicleDistanceToTargetCalculator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private Transform target;
        [SerializeField] private Transform policeVehicle;
        [SerializeField] private GameObject _policeCarUI;
        [SerializeField] private float _minDistanceToActivateCanvas = 8;

        private bool _isInPuzzleGameStarter = false;

        private void Awake()
        {
            _policeCarUI.SetActive(true);
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger += OnPuzzleGameStarter;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger -= OnPuzzleGameStarter;
        }

        private void Update()
        {
            var distance = Vector3.Distance(policeVehicle.position, target.position);
            
            if (distance < _minDistanceToActivateCanvas)
            {
                
                if(_policeCarUI.activeSelf)
                    _policeCarUI.SetActive(false);
            }
            else if (distance > 8 && !_isInPuzzleGameStarter)
            { 
                if(!_policeCarUI.activeSelf)
                _policeCarUI.SetActive(true);
            }
            
            distanceText.text = distance.ToString("F2", CultureInfo.InvariantCulture) + "m";
        }
        
        private void OnPuzzleGameStarter()
        {
            _isInPuzzleGameStarter = true;
        }
    }
}