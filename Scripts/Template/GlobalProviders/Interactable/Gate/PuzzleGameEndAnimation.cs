using System;
using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Gate
{
    public class PuzzleGameEndAnimation : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        
        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _playableDirector.playOnAwake = false;
        }
        
        private void OnEnable()
        {
            EventManager.InGameEvents.PuzzleGameEnd += OnPuzzleGameEnd;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.PuzzleGameEnd -= OnPuzzleGameEnd;
        }

        private void OnPuzzleGameEnd()
        {
            _playableDirector.Play();
        }
    }
}