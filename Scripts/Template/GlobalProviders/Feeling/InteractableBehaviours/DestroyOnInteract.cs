using _Game.Scripts.Template.GlobalProviders.Interactable;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.InteractableBehaviours
{
    public class DestroyOnInteract : MonoBehaviour, IInteractableAction
    {
        public void InteractableAction()
        {
            transform.DOKill(transform);
            Destroy(gameObject);
        }
    }
}