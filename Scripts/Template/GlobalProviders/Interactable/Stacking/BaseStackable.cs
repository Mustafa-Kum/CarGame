using System;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Stacking
{
    public abstract class BaseStackable : MonoBehaviour, IInteractableAction
    {
        #region Inspector Variables

        [SerializeField] private bool CanStack { get; set; } = true;

        public StackableData stackableDataStruct;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelLoaded += GetDefaults;
            EventManager.StackableEvents.Unstack += CallUnstack;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelLoaded -= GetDefaults;
            EventManager.StackableEvents.Unstack -= CallUnstack;   
        }

        #endregion

        #region Private Methods
        
        private void GetDefaults(GameObject arg0)
        {
            if (stackableDataStruct.StackableObject == null)
                stackableDataStruct.StackableObject = gameObject;
        }
        
        private void CallStack()
        {
            if (!CanStack) return;
    
            EventManager.StackableEvents.Stack?.Invoke(stackableDataStruct);
    
            OnStacking(gameObject);
        }

        private void CallUnstack(StackableData _stackableData)
        {
            if (!CanStack) return;
            
            OnUnstacking(gameObject);
        }

        #endregion
        
        #region Public Methods

        public void InteractableAction()
        {
            CallStack();
        }
        
        #endregion

        #region Abstract Methods

        protected abstract void OnStacking(GameObject stackable);
        protected abstract void OnUnstacking(GameObject stackable);

        #endregion
    }
    
    [Serializable]
    public struct StackableData
    {
        public GameObject StackableObject;
        public int StackableObjectAmount;
        public StackableType stackableType;

        public enum StackableType
        {
            StackableType1,
            StackableType2,
            StackableType3,
        }
    }
}