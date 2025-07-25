using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Combat;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Health
{
    [RequireComponent(typeof(DamageableObject))]
    public abstract class HealthProvider : MonoBehaviour, IHealth, IDamageableAction
    {
        #region Public Variables

        [SerializeField] protected DamageableData _damageableData;
        public float CurrentHealth => _damageableData.currentHealth;

        #endregion

        #region Private Variables

        private DamageableObject _damageableObject;

        #endregion

        #region Virtual Methods

        protected virtual void OnDeath() => _damageableObject.CanInteract = false;

        public virtual void TakeDamage(float damage)
        {
            _damageableData.currentHealth -= damage;
            OnHealthChanged(_damageableData.currentHealth);

            if (_damageableData.currentHealth <= 0)
            {
                _damageableObject.Death();
            }
        }

        #endregion

        #region Private Methods

        private void OnHealthChanged(float newHealth)
        {
            _damageableObject.DamageableHealthChanged(newHealth);
        }

        #endregion

        #region Public Methods
        
        public void Death() => OnDeath();

        public void Initialize(DamageableObject damageableObject)
        {
            this._damageableObject = damageableObject;
            
            _damageableData.currentHealth = _damageableData.maxHealth;
            
            damageableObject.damageableData = _damageableData;
        }

        public virtual void Heal(float amount)
        {
            _damageableData.currentHealth = Mathf.Min(_damageableData.currentHealth + amount, _damageableData.maxHealth);
            OnHealthChanged(_damageableData.currentHealth);
        }
        
        public void HealthChanged(float currentHealth){}

        #endregion
    }
}