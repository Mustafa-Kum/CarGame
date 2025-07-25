using System.Collections;
using UnityEngine;
using _Game.Scripts.InGame.Controllers;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Upgrade;

namespace _Game.Scripts._GameLogic.Logic
{
    public class VehicleSpeedBoostController : MonoBehaviour
    {
        [SerializeField] private GameObject vehicleSpeedBoostEffect;
        [SerializeField] private CharacterMover characterMover;
        [SerializeField] private PlayerUpgradeData playerUpgradeData;
        
        [SerializeField] private BoostSpeedDurationUpgradeSO speedBoostDurationUpgradeSO;
        [SerializeField] private BoostSpeedValueUpgradeSO boostSpeedUpgradeSO;
        [SerializeField] private SpeedReduceUpgradeSO speedReduceUpgradeSO;
        [SerializeField] private VehicleSpeedBoostValues vehicleSpeedBoostValues;
        
        private Coroutine speedBoostCoroutine;
        private Coroutine speedReduceCoroutine;
        private float remainingSpeedBoostTime;
        private float remainingSpeedReduceTime;
        
        
        private float currentBoostSpeedDuration;
        private float currentBoostSpeedValue;
        private float currentSpeedReduceValue;
        
        private void Awake()
        {
            vehicleSpeedBoostEffect.SetActive(false);
        }

        private void OnEnable()
        {
            EventManager.InteractableEvents.SpeedBoostInteract += OnSpeedBoostInteract;
            EventManager.InteractableEvents.SpeedReduceInteract += OnSpeedReduceInteract;
            EventManager.InGameEvents.LevelStart += CacheSpeedUpgradeData;
        }

        private void OnDisable()
        {
            EventManager.InteractableEvents.SpeedBoostInteract -= OnSpeedBoostInteract;
            EventManager.InteractableEvents.SpeedReduceInteract -= OnSpeedReduceInteract;
            EventManager.InGameEvents.LevelStart += CacheSpeedUpgradeData;
        }
        
        private void CacheSpeedUpgradeData()
        {
            currentBoostSpeedValue = boostSpeedUpgradeSO.GetValue(playerUpgradeData.GetUpgradeLevel(UpgradeType.SpeedBoostValue));
            currentBoostSpeedDuration = speedBoostDurationUpgradeSO.GetValue(playerUpgradeData.GetUpgradeLevel(UpgradeType.SpeedBoostDuration));
            currentSpeedReduceValue = speedReduceUpgradeSO.GetValue(playerUpgradeData.GetUpgradeLevel(UpgradeType.SpeedReduceValue));
        }

        private void OnSpeedBoostInteract()
        {
            var speedBoostDuration = currentBoostSpeedDuration;
            
            if (speedBoostCoroutine != null)
            {
                remainingSpeedBoostTime = Mathf.Max(remainingSpeedBoostTime, speedBoostDuration);
            }
            else
            {
                remainingSpeedBoostTime = speedBoostDuration;
                speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(currentBoostSpeedValue));
            }
        }
        
        private void OnSpeedReduceInteract()
        {
            var speedReduceDuration = vehicleSpeedBoostValues.GetSpeedReduceDuration();
            
            if (speedReduceCoroutine != null)
            {
                remainingSpeedReduceTime = Mathf.Max(remainingSpeedReduceTime, speedReduceDuration);
            }
            else
            {
                remainingSpeedReduceTime = speedReduceDuration;
                speedReduceCoroutine = StartCoroutine(SpeedReduceCoroutine(currentSpeedReduceValue));
            }
        }

        private IEnumerator SpeedBoostCoroutine(float speedBoostValue)
        {
            ActivateSpeedBoostEffect(remainingSpeedBoostTime);
            characterMover.speed += speedBoostValue;
            vehicleSpeedBoostValues.ModifySpeedBoost(true);

            while (remainingSpeedBoostTime > 0)
            {
                yield return null;
                remainingSpeedBoostTime -= Time.deltaTime;
            }

            characterMover.speed -= speedBoostValue;
            vehicleSpeedBoostValues.ModifySpeedBoost(false);
            speedBoostCoroutine = null;
        }

        private IEnumerator SpeedReduceCoroutine(float speedReduceValue)
        {
            characterMover.speed -= speedReduceValue;

            while (remainingSpeedReduceTime > 0)
            {
                yield return null;
                remainingSpeedReduceTime -= Time.deltaTime;
            }

            characterMover.speed += speedReduceValue;
            speedReduceCoroutine = null;
        }

        private void ActivateSpeedBoostEffect(float duration)
        {
            vehicleSpeedBoostEffect.SetActive(true);
            StartCoroutine(DeactivateSpeedBoostEffect(duration));
        }

        private IEnumerator DeactivateSpeedBoostEffect(float duration)
        {
            yield return new WaitForSeconds(duration);
            vehicleSpeedBoostEffect.SetActive(false);
        }
    }
}