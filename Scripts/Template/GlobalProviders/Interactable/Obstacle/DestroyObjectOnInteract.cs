﻿using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Obstacle
{
    public class DestroyObjectOnInteract : MonoBehaviour, IInteractableAction
    {
        #region Public Methods

        public void InteractableAction()
        {
            Destroy(gameObject);
        }
        
        #endregion
    }
}