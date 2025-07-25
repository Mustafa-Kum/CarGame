using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Predefined;
using _Game.Scripts.ScriptableObjects.Saveable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Collectables;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _Game.Scripts.UI.Buttons
{
    public class UpgradeButton : ButtonBase
    {
        [SerializeField] private UpgradableSO _upgradableSO;
        [SerializeField] private UpgradeButtonStruct _upgradeButtonStruct;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI upgradeLevelText;
        [SerializeField] private Image costIconImage;
        [SerializeField] private CurrencyValuesSO currencyValuesSo;
        [SerializeField] private IconProviderSO iconProvider;
        [SerializeField] private Image rewardedAdImage;
        [SerializeField] private PlayerUpgradeData _playerUpgradeData;
        [SerializeField] private TextMeshProUGUI _upgradeTypeText;
        [SerializeField] private ParticleImage particleImage;
        [SerializeField] private float calmdownTime = 1f;
        [SerializeField]  private Color disabledColor;
        [SerializeField] private List<ParticleSystem> _carParticles;
        
        private bool _isRewardedAdActive;
        private bool _isInCooldown;
        private Color originalColor;
   
        
        
        private void OnEnable()
        {
            SubscribeEvents();
            ResetButtonProperties();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.CurrencySystem.CollectableSpent += OnCollectableSpent;
            EventManager.UpgradeSystem.CharacterUpgraded += OnCharacterUpgraded;
        }

        private void UnsubscribeEvents()
        {
            EventManager.CurrencySystem.CollectableSpent -= OnCollectableSpent;
            EventManager.UpgradeSystem.CharacterUpgraded -= OnCharacterUpgraded;
        }

        private void OnCharacterUpgraded(UpgradeType upgradeType)
        {
            if (upgradeType == _upgradableSO.upgradeType)
            {
                ResetButtonProperties();
            }
        }

        private void OnCollectableSpent(CollectableType collectableType)
        {
            if (collectableType == _upgradableSO.collectableType)
            {
                ResetButtonProperties();
            }
        }

        private void SetIcon()
        {
            iconImage.sprite = iconProvider.GetUpgradeIcon(_upgradableSO.upgradeType);
        }

        private void SetCostAndCostImage()
        {
            int cost = _upgradableSO.GetRequiredCurrencyForNextLevel(_playerUpgradeData.GetUpgradeLevel(_upgradableSO.upgradeType));
            costText.text = cost.ToString();

            Sprite costSprite = iconProvider.GetCollectableIcon(_upgradableSO.GetCollectableType(_playerUpgradeData.GetUpgradeLevel(_upgradableSO.upgradeType)));
            costIconImage.sprite = costSprite;
        }

        private void SetUpgradeTypeText()
        {
            _upgradeTypeText.text = _upgradableSO.upgradeType.ToString();
        }

        private void CheckAvailability()
        {
            int cost = _upgradableSO.GetRequiredCurrencyForNextLevel(_playerUpgradeData.GetUpgradeLevel(_upgradableSO.upgradeType));
            if (cost <= currencyValuesSo.GetValue(_upgradableSO.collectableType))
            {
                TryActivateRewardedAdImage(false);
                _isRewardedAdActive = false;
                GetComponent<Button>().image.color = originalColor;
            }
            else
            {
                _isRewardedAdActive = true;
                SetRewardedAdState();
                GetComponent<Button>().image.color = disabledColor;
            }
        }

        private void SetRewardedAdState()
        {
            TryActivateRewardedAdImage(true);
        }

        private void OnRewardedAdClicked()
        {
            Debug.Log("OnRewardedAdClicked -- 55");
            EventManager.AdEvents.ShowRewarded?.Invoke(OnRewardedAdCompleted);
        }

        private void OnRewardedAdCompleted()
        {
            Debug.Log("OnRewardedAdCompleted -- 66");

            _upgradeButtonStruct.fromRewardedAd = true;
            EventManager.UpgradeSystem.UpgradeButtonClicked?.Invoke(_upgradableSO.upgradeType, _upgradeButtonStruct);
            _upgradeButtonStruct.fromRewardedAd = false;
            TryActivateRewardedAdImage(false);
            ResetButtonProperties();
        }

        [Button]
        public void ResetButtonProperties()
        {
            SetIcon();
            SetCostAndCostImage();
            CheckAvailability();
            SetUpgradeTypeText();
            SetUpgradeLevelText();
            _upgradeButtonStruct.increaseCount = 1;
            if (_isInCooldown)
            {
                return;
            }
            originalColor = Color.white;
            transform.DOScale(Vector3.one, 0.2f);
        }

        private void SetUpgradeLevelText()
        {
            upgradeLevelText.text = "LVL " + _playerUpgradeData.GetUpgradeLevel(_upgradableSO.upgradeType).ToString();
        }

        protected override void OnClicked()
        {
            if (_isInCooldown)
            {
                return;
            }

            if (_isRewardedAdActive)
            {
                Debug.Log("Rewarded Ad Active");
                OnRewardedAdClicked();
                return;
            }

            EventManager.UpgradeSystem.UpgradeButtonClicked?.Invoke(_upgradableSO.upgradeType, _upgradeButtonStruct);
            particleImage.Play();
            ChangeParticleColors(_carParticles, 100);
            transform.DOScale(Vector3.one * 1.1f, 0.2f).OnComplete(() => 
            {
                transform.DOScale(Vector3.one, 0.2f);
                StartCoroutine(Cooldown());
            });
        }
        
        private void ChangeParticleColors(List<ParticleSystem> particleSystems, float alpha)
        {
            foreach (var particleSystem in particleSystems)
            {
                var main = particleSystem.main;
                Color newColor = main.startColor.color;
                newColor.a = alpha / 255f; // alpha should be between 0 and 1
                main.startColor = newColor;
            }
        }

        private IEnumerator Cooldown()
        {
            _isInCooldown = true;
            yield return new WaitForSeconds(calmdownTime);
            //particleImage.SetActive(false);
            ChangeParticleColors(_carParticles, 0);
            _isInCooldown = false;
        }

        private void TryActivateRewardedAdImage(bool isRewardedAdActive)
        {
            if (rewardedAdImage != null)
            {
                rewardedAdImage.gameObject.SetActive(isRewardedAdActive);
            }
        }
    }

    [Serializable]
    public struct UpgradeButtonStruct
    {
        public bool isResettable;
        public bool fromRewardedAd;
        public int increaseCount;
    }
}
