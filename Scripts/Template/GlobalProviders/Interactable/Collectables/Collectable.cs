using System;
using _Game.Scripts.Managers.Core;
using Handler.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Collectables
{
    public sealed class Collectable : MonoBehaviour, IInteractableAction
    {
        #region Public Variables
        
        public CollectableData collectableData;
        
        #endregion

        #region Private Variables
        private bool CanCollect { get; set; } = true;

        #endregion

        #region Unity Methods

        private void Awake() => collectableData.CollectableGO = gameObject;
        
        #endregion
        
        #region Public Methods

        public void InteractableAction()
        {
            if (!CanCollect) return;
            
            EventManager.CollectableEvents.Collect?.Invoke(new CollectableData
            {
                Type = collectableData.Type,
                ScoreAmount = collectableData.ScoreAmount,
                CollectableGO = collectableData.CollectableGO,
                collectedPosition = transform.position
            });
        }

        #endregion
    }
    
    [Serializable]
    public struct CollectableData
    {
        [FormerlySerializedAs("Collectable")] public GameObject CollectableGO;
        public CollectableType Type;
        public GridObjectType GridObjectType;
        public int ScoreAmount;
        [HideInInspector] public Vector3 collectedPosition;
    }
    
    [Serializable]
    public enum CollectableType
    {
        Coin,
        Gem,
    }
}