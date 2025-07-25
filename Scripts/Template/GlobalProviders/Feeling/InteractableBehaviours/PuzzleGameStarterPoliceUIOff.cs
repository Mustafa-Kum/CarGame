using System;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.InteractableBehaviours
{
    public class PuzzleGameStarterPoliceUIOff : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger += OnPuzzleGameStarter;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger -= OnPuzzleGameStarter;
        }
        
        private void OnPuzzleGameStarter()
        {
            gameObject.SetActive(false);
        }
    }
}