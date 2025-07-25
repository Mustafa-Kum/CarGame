using System;
using System.Collections.Generic;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Predefined;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using Sirenix.OdinInspector;
using UnityEngine;
using Fluxy;
using TMPro;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Gate
{
    public abstract class BaseGateInteractable : MonoBehaviour, IInteractableAction, ILevelObject
    {
        #region Public Variables

        //[OnValueChanged("InitiateSetGateData")] [OnValueChanged("InitiateSetColorData")]
        public GateInteractableData gateInteractableData;

        [SerializeField] public bool CanInteract;
        [SerializeField] public bool _isGuardedGate;
        [SerializeField] public TextMeshPro _upgradeText;
        public GuardedGateData guardedGateData;

        public FluxyContainer Container;
        public float shatterDisableTime = 3f;
        public MeshRenderer gateMeshRenderer;
        
        public bool isGateOpen;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SetThisObject();
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        #endregion

        #region Private Methods

        private void Subscribe()
        {
            EventManager.ShootableEvents.BulletFluxyFluidOnShoot += InitiateFluxyData;
        }

        private void UnSubscribe()
        {
            EventManager.ShootableEvents.BulletFluxyFluidOnShoot -= InitiateFluxyData;
        }

        private void SetThisObject()
        {
            if (gateInteractableData.InteractableGameObject != null) return;
            gateInteractableData.InteractableGameObject = gameObject;
        }

        #endregion

        #region Public Methods
        public void InteractableAction()
        {
            Interact();
        }

        #endregion

        #region Abstract Methods
        protected abstract void OnGateInteraction();

        #endregion

        #region Private Methods
        private void Interact()
        {
            if (!CanInteract) return;

            EventManager.InteractableEvents.GateInteract?.Invoke(new GateInteractableData
            {
                InteractableGameObject = gateInteractableData.InteractableGameObject,
                Amount = gateInteractableData.Amount,
                mathType = gateInteractableData.mathType,
                UpgradeType = gateInteractableData.UpgradeType
            });

            CanInteract = false;

            OnGateInteraction();
        }
        
        private void InitiateFluxyData(FluxyTarget target)
        {
            if (Container == null) return;

            TDebug.Log(target);
            
            List<FluxyTarget> targets = new List<FluxyTarget>();
            
            targets.Add(target);

            Container.targets = targets.ToArray();
        }
        
        public void ApplyModifier(string csvModifierValue)
        {
            if (csvModifierValue.StartsWith("+"))
            {
                this.gateInteractableData.mathType = MathType.Add;
                if (int.TryParse(csvModifierValue.Substring(1), out int amount))
                {
                    this.gateInteractableData.Amount = amount;
                }
            }
        }

        #endregion
    }

    [Serializable]
    public struct GateInteractableData
    {
        public GameObject InteractableGameObject;

        public int Amount;

        public MathType mathType;

        public UpgradeType UpgradeType;
    }

    [Serializable]
    public struct GuardedGateData
    {
        public int CurrentAmount;

        public int MaxAmount;

        public List<GameObject> GuardedGateObjects;
    }

    [EnumToggleButtons]
    public enum MathType
    {
        Add,
        Subtract,
        Divide,
        Multiply
    }
}