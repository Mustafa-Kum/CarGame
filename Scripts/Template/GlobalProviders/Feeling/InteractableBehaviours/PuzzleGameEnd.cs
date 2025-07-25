using _Game.Scripts.Managers.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.InteractableBehaviours
{
    public class PuzzleGameEnd : MonoBehaviour
    {
        [Button]   
        private void OnPuzzleGameEnd()
        {
            EventManager.InGameEvents.PuzzleGameEnd?.Invoke();
        }
    }
}