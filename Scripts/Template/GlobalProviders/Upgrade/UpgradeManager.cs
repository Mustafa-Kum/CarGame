using System;
using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using _Game.Scripts.Template.GlobalProviders.Interactable.Gate;
using _Game.Scripts.UI.Buttons;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Upgrade
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private List<UpgradableSO> upgradableSOs;
        [SerializeField] private PlayerUpgradeData playerUpgradeData;
        private readonly Dictionary<UpgradeType, int> _initialUpgrades = new Dictionary<UpgradeType, int>();

        [SerializeField] private ParticleSystem _upgradePartical;

        private void OnEnable()
        {
            SubscribeEvents();
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        private void SubscribeEvents()
        {
            EventManager.UpgradeSystem.UpgradeButtonClicked += OnUpgradeButtonClicked;
            EventManager.InGameEvents.LevelSuccess += ResetUpgrades;
            EventManager.InGameEvents.LevelStart+= CacheUpgradeData;
            EventManager.InteractableEvents.GateInteract += HandleWithGateInteract;
            EventManager.InteractableEvents.BoostInteract += InteractWithBoost;
        }

        private void UnsubscribeEvents()
        {
            EventManager.UpgradeSystem.UpgradeButtonClicked -= OnUpgradeButtonClicked;
            EventManager.InGameEvents.LevelSuccess -= ResetUpgrades;
            EventManager.InteractableEvents.GateInteract -= HandleWithGateInteract;
            EventManager.InteractableEvents.BoostInteract -= InteractWithBoost;
            EventManager.InGameEvents.LevelStart-= CacheUpgradeData;


        }
        private void CacheUpgradeData()
        {
            foreach (var upgradable in upgradableSOs)
            {
                var upgradeType = upgradable.upgradeType;
                var currentValue = playerUpgradeData.GetUpgradeLevel(upgradeType);
                _initialUpgrades[upgradeType] = currentValue;
                
                Debug.Log("CacheUpgradeData: " + upgradeType + " " + currentValue);
            }
        }

        private void HandleWithGateInteract(GateInteractableData data)
        {
            var upgradeType = data.UpgradeType;

            float currentValue = playerUpgradeData.GetUpgradeLevel(upgradeType);

            var upgradedLevel = ConvertGateValueToUpgradeValue(data.mathType, data.Amount, (int)currentValue);
          
            playerUpgradeData.SetUpgradeLevel(upgradeType, upgradedLevel);
           
        }




        private void InteractWithBoost(BoostInteractableData data)
        {
            var upgradeType = data.UpgradeType;
            
            var upgradedLevel = ConvertBoostValueToUpgradeValue();
            playerUpgradeData.IncreaseUpgradeLevel(upgradeType, upgradedLevel);

        }


        private int ConvertGateValueToUpgradeValue(MathType mathType, int gateValue, int currentLevel)
        {
            var upgradeValue = 1;

            switch(mathType)
            {
                case MathType.Add:
                    case MathType.Multiply:
                    return currentLevel + upgradeValue;
                case MathType.Subtract:
                    case MathType.Divide:
                    return currentLevel - gateValue;

                default:
                    return currentLevel;
            }
        }

        private int ConvertBoostValueToUpgradeValue()
        {
            return 1;
        }

        private void OnUpgradeButtonClicked(UpgradeType upgradeType, UpgradeButtonStruct upgradeButtonStruct)
        {

            if (upgradeButtonStruct.isResettable)
            {
                if (!_initialUpgrades.ContainsKey(upgradeType))
                {
                   //TODO: Resetable Upgrade From Button
                }
            }

            if (upgradeButtonStruct.fromRewardedAd)
            {
                Debug.Log("From Rewarded Ad");
                
                PerformUpgrade(upgradeType, upgradeButtonStruct.increaseCount);
                EventManager.UpgradeSystem.CharacterUpgraded?.Invoke(upgradeType);
                TryActivateUpgradePartical();
                return;
            }

            var upgradeCost = GetUpgradeCost(upgradeType);
            var collectableType = GetCollectableType(upgradeType);
            EventManager.CurrencySystem.TryToBuy?.Invoke(upgradeCost, collectableType, isBuySuccessful =>
            {
                if (isBuySuccessful)
                {
                    PerformUpgrade(upgradeType, upgradeButtonStruct.increaseCount);
                    EventManager.UpgradeSystem.CharacterUpgraded?.Invoke(upgradeType);
                    TryActivateUpgradePartical();
                }
            });
        }


        private CollectableType GetCollectableType(UpgradeType upgradeType)
        {
            foreach (var upgradable in upgradableSOs)
            {
                if (upgradable.upgradeType == upgradeType)
                {
                    return upgradable.GetCollectableType(playerUpgradeData.GetUpgradeLevel(upgradeType));
                }
            }
            return CollectableType.Coin;
        }

        private int GetUpgradeCost(UpgradeType upgradeType)
        {
            foreach (var upgradable in upgradableSOs)
            {
                if (upgradable.upgradeType == upgradeType)
                {
                    return upgradable.GetRequiredCurrencyForNextLevel(playerUpgradeData.GetUpgradeLevel(upgradeType));
                }
            }
            return 0;
        }

        private void PerformUpgrade(UpgradeType upgradeType, int increaseCount)
        {
            playerUpgradeData.IncreaseUpgradeLevel(upgradeType);
        }

        private void ResetUpgrades()
        {
            foreach (var upgradable in upgradableSOs)
            {
                var upgradeType = upgradable.upgradeType;
                var initialValue = _initialUpgrades[upgradeType];
                playerUpgradeData.SetUpgradeLevel(upgradeType, initialValue);
                    Debug.Log("ResetUpgrades: " + upgradeType + " " + initialValue);
                    
            }
            
            
            _initialUpgrades.Clear();
        }
        
        private void TryActivateUpgradePartical()
        {
            if (_upgradePartical != null)
                _upgradePartical.Play();
        }

    }
    [Serializable]
    public enum UpgradeType
    {
        BaseSpeedValue,
        SpeedBoostValue,
        SpeedBoostDuration,
        SpeedReduceValue,
    }
}
