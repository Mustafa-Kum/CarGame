using _Game.Scripts.General;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Combat;
using Handler.Extensions;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.EndMeta
{
    public class EndMetaObstacle : MonoBehaviour, IInteractableAction, IDamageableAction
    {
        #region Private Fields

        [SerializeField] private TextMeshPro _healthText;
        
        #endregion

        #region Public Methods

        public void InteractableAction()
        {
            EventManager.InGameEvents.LevelSuccess.Invoke();
        }

        public void Initialize(DamageableObject damageableObject)
        {
            if (_healthText == null)
            {
                _healthText = GetComponentInChildren<TextMeshPro>();
            }
            
            _healthText.text = $"{damageableObject.damageableData.currentHealth.ToInt()}";
        }

        public void TakeDamage(float damage)
        {
            
        }

        public void Death()
        {
            Destroy(gameObject);
        }
        
        public void HealthChanged(float currentHealth)
        {
            _healthText.text = $"{currentHealth.ToInt()}";
        }

        #endregion
    }
}
