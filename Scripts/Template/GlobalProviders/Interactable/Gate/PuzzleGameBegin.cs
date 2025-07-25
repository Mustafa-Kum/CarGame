using System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Tutorial.Logic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Gate
{
    public class PuzzleGameBegin : MonoBehaviour
    {
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.PuzzleGameBegin += OnPuzzleGameBegin;
            EventManager.TutorialEvents.TutorialCompleted += OnPuzzleGameEnd;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.PuzzleGameBegin -= OnPuzzleGameBegin;
            EventManager.TutorialEvents.TutorialCompleted -= OnPuzzleGameEnd;
        }

        private void OnPuzzleGameBegin()
        {
            DOVirtual.DelayedCall(3f, () => _playableDirector.Play());
            EventManager.InGameEvents.PuzzleTableAscend?.Invoke();
            
            if(TutorialManager.m_Instance.playerSavableData.LevelIndex > 1)
                DOVirtual.DelayedCall(15f, () => EventManager.InGameEvents.PuzzleGameEnd?.Invoke());
        }
        
        private void OnPuzzleGameEnd()
        {
            if(TutorialManager.m_Instance.stepsByLevel[0].isTutorialCompleted && TutorialManager.m_Instance.playerSavableData.LevelIndex == 1)
                DOVirtual.DelayedCall(15f, () => EventManager.InGameEvents.PuzzleGameEnd?.Invoke());
        }
    }
}