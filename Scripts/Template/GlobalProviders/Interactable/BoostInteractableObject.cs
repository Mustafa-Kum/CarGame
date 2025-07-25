using System;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable
{
    public class BoostInteractableObject : MonoBehaviour, IInteractableAction
    {
        [SerializeField] private BoostInteractableData _boostInteractableData;
        
        public void InteractableAction()
        {
            EventManager.InteractableEvents.BoostInteract?.Invoke(_boostInteractableData);
            
            Destroy(gameObject);
        }
    }
    
    [Serializable]
    public struct BoostInteractableData
    {
        public int Amount;
        public UpgradeType UpgradeType;
    }
}
