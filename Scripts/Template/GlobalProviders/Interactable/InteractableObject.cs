using System;
using _Game.Scripts.Managers.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        #region Serialized Fields

        [SerializeField] public bool canInteract = true;
        
        [SerializeField] public bool isLazyInitialized = true;

        [ShowInInspector] public IInteractableAction[] actions;
        
        [SerializeField] public InteractableData interactableData;

        #endregion
        
        #region Private Fields
        
        public bool isInitialized;

        #endregion

        #region Public Properties

        public bool CanInteract => canInteract;

        #endregion

        #region Private Methods

        public void EnsureInitialized()
        {
            if (isInitialized) return;
    
            actions = GetComponentsInChildren<IInteractableAction>();
            isInitialized = true;
        }


        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (isLazyInitialized) return;
            EnsureInitialized();
        }

        #endregion

        #region Public Methods

        public virtual void Interact()
        {
            EnsureInitialized();
            
            if (!CanInteract || !isInitialized || actions == null || actions.Length == 0) return;
    
            EventManager.InteractableEvents.Interact?.Invoke(interactableData);

            foreach (var action in actions)
            {
                action.InteractableAction();
            }
            canInteract = false;
        }


        #endregion
    }

    #region Data Structures

    [Serializable]
    public struct InteractableData
    {
        public float Amount;
    }

    #endregion
}