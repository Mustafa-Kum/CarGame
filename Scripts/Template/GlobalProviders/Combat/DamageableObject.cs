using System;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using Handler.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    public class DamageableObject : MonoBehaviour, IDamageable
    {
        #region Inspector Variables

        [ShowInInspector] private IDamageableAction[] actions;
        
        [SerializeField] private bool canInteract = true;

        public DamageableData damageableData;
        
        #endregion

        public bool CanInteract 
        { 
            get => canInteract; 
            set => canInteract = value; 
        }

        #region Unity Methods

        private void Awake()
        {
            actions = GetComponentsInChildren<IDamageableAction>();
            
            foreach (var action in actions)
            {
                action.Initialize(this);
            }
        }
        
        #endregion
        
        public void TakeDamage(float damage)
        {
            if (!CanInteract) return;

            if (actions==null)
            {
                TDebug.LogError("No IDamageableAction found on " + gameObject.name);
                return;
            }
            
            foreach (var action in actions)
            {
                action.TakeDamage(damage);
            }
        }
        
        public void Death()
        {
            EventManager.HealthEvents.DamageableDeath.Invoke(damageableData);

            CanInteract = false;
            foreach (var action in actions)
            {
                action.Death();
            }
        }
        
        public void DamageableHealthChanged(float currentHealth)
        {
            foreach (var action in actions)
            {
                action.HealthChanged(currentHealth);
            }
        }
    }
    
    [Serializable]
    public struct DamageableData
    {
        [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public float maxHealth;
        [SerializeField] public float currentHealth;
        
        public enum DamageableType
        {
            Player,
            Enemy,
            Environment
        }
        
        [GUIColor(1f, 0.3f, 0.3f, 1f)]
        public DamageableType damageableType;
    }
}