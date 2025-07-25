using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Gate
{
    public class PuzzleGameStarter : MonoBehaviour, IInteractableAction
    {
        public void InteractableAction()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger?.Invoke();
        }
    }
}