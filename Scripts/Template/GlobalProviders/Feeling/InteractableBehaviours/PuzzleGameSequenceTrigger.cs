using _Game.Scripts.InGame.Controllers;
using _Game.Scripts.InGame.ReferenceHolder;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Feeling.InteractableBehaviours
{
    public class PuzzleGameSequenceTrigger : MonoBehaviour
    {
        [SerializeField] private CharacterMover _characterMover;
        [SerializeField] private GameObject _carHolder;
        
        private Transform _puzzleCarTransform;
        private Transform _garageTransform;
        private GameObject _carHight;

        private void OnEnable()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger += OnPuzzleGameStart;
            EventManager.InGameEvents.PuzzleGameEnd += OnPuzzleGameEnd;
            EventManager.InGameEvents.LevelLoaded += OnLevelLoaded;
        }
        
        private void OnDisable()
        {
            EventManager.InGameEvents.PuzzleGameTransformTrigger -= OnPuzzleGameStart;
            EventManager.InGameEvents.PuzzleGameEnd -= OnPuzzleGameEnd;
            EventManager.InGameEvents.LevelLoaded -= OnLevelLoaded;
        }
        
        private void OnPuzzleGameStart()
        {
            DOVirtual.DelayedCall(5f, () =>
            {
                EventManager.InGameEvents.PuzzleGameBegin?.Invoke();
            });

            transform.DOMove(new Vector3(_puzzleCarTransform.position.x, transform.position.y, _puzzleCarTransform.position.z), 8f).OnComplete(() =>
            {
                _characterMover.enabled = false;
            });
            transform.DORotate(new Vector3(0, 0, 0), 8f);
        }
        
        private void OnPuzzleGameEnd()
        {
            DOVirtual.DelayedCall(3f, () =>
            {
                transform.DOMove(new Vector3(_garageTransform.position.x, transform.position.y, _garageTransform.position.z), 8f).OnComplete(() =>
                {
                    _carHight.transform.DOMoveY(-3f, 2f);
                    _carHolder.transform.DORotate(new Vector3(0, 180, 0), 3f);
                    transform.DOMoveY(0.418f, 2f);
                });
            });
        }
        
        private void OnLevelLoaded(GameObject arg0)
        {
            _characterMover.enabled = true;
            
            var levelReferenceHolder = arg0.GetComponent<LevelReferenceHolder>();
            
            _puzzleCarTransform = levelReferenceHolder.PuzzleCarTransform;
            _garageTransform = levelReferenceHolder.GarageTransform;
            _carHight = levelReferenceHolder.CarHight;
        }
    }
}