using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PathMove : MonoBehaviour
    {
        #region SerializeField

        [SerializeField] private float _yOffset;
        [SerializeField] private float _duration;
        [SerializeField] private float _delay;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private ParticleSystem _endParticle;
        [SerializeField] private ParticleSystem _sandParticle;
        [SerializeField] private Transform _finishTransform;
        [SerializeField] private bool _isBall;

        #endregion

        private Vector3 _targetPosition;
        private Sequence _lastSequence;
        private Sequence _sequence;
        
        private void OnEnable()
        {
            EventManager.GridEvents.OnPathFound += MovingOnPath;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnPathFound -= MovingOnPath;
        }

        private void MovingOnPath(List<GridTile> pathCubes)
        {
            KillSequencesBeforeSequencePlay();
            StartCoroutine(FollowPath(pathCubes));
        }

        private IEnumerator FollowPath(List<GridTile> pathCubes)
        {
            yield return new WaitForSeconds(_delay);
    
            transform.rotation = Quaternion.identity;
            
            // Moving Obje Animation on Per Cube;
            
            for (int currentIndex = 0; currentIndex < pathCubes.Count; currentIndex++)
            {
                if (currentIndex < pathCubes.Count - 1)
                {
                    GetComponentsInChildren<Animator>()[0].ResetTrigger("Jump");
                    GetComponentsInChildren<Animator>()[0].SetTrigger("Jump");
                    
                    _targetPosition = pathCubes[currentIndex].transform.position;
                    _targetPosition.y += _yOffset;

                    Sequence sequence = ObjectAnimationWhileMoving();
            
                    pathCubes[currentIndex].PathAction();
            
                    yield return sequence.Play().WaitForCompletion();
                }
                
                // Moving Obje Animation on Last Cube;
                
                if (currentIndex == pathCubes.Count - 1)
                {
                    _targetPosition = pathCubes[currentIndex].transform.position;
                    _targetPosition.y += _yOffset;
                    
                    Sequence lastSequence = ObjectLastAnimation();
                    
                    pathCubes[currentIndex].PathAction();
        
                    yield return lastSequence.Play().WaitForCompletion();
                }
            }
        }

        private Sequence ObjectAnimationWhileMoving()
        {
            _sequence = DOTween.Sequence();
            
            Vector3 direction = transform.position - _targetPosition;
            
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            Vector3 eulerRotation = lookRotation.eulerAngles;
            
            transform.DORotate(eulerRotation, _duration).SetEase(Ease.OutQuad);
            
            _sequence.Append(
                transform.DOJump
                    (new Vector3(_targetPosition.x, _targetPosition.y, _targetPosition.z), _jumpHeight, 1, _duration)
                    .SetEase(Ease.OutQuad)
            );

            if (_isBall == true)
            {
                _sequence.Join(
                    transform.transform.DORotate
                        (new Vector3(360, 360, 360), 0.4f, RotateMode.FastBeyond360));
            }

            return _sequence;
        }

        private Sequence ObjectLastAnimation()
        {
            EventManager.AudioEvents.AudioPlay?.Invoke(SoundType.PathSuccess, true, false);
            
            _lastSequence = DOTween.Sequence();

            _lastSequence.Append
                (transform.DOJump
                    (new Vector3(_targetPosition.x, _targetPosition.y, _targetPosition.z), _jumpHeight, 1, _duration)
                .SetEase(Ease.OutQuad)
                );
            
            _lastSequence.Append
                (transform.DOJump
                    (new Vector3(_finishTransform.position.x, _finishTransform.position.y, _finishTransform.position.z - 0.2f), 
                        2, 1, _duration).SetDelay(0.5f).OnComplete((() =>
                        {
                            ParticalPlayer(_sandParticle);
                        }))
                .SetEase(Ease.OutQuad)
                );
 
            _lastSequence.Join
                (transform.DORotate(new Vector3(0, 0, 0), 0.4f)
                    .SetEase(Ease.OutQuad)
                );
            
            _lastSequence.Append
                (transform.DOScale(Vector3.one, 0.2f).OnComplete((() =>
                    {
                        ParticalPlayer(_endParticle);
                    }))
                    .SetEase(Ease.OutQuad)
                );
            
            _lastSequence.Append
                (transform.DOScale(Vector3.one * 0.54745f, 0.2f)
                    .SetEase(Ease.OutQuad)
                );
            
            return _lastSequence;
        }

        private void ParticalPlayer(ParticleSystem particle)
        {
            if (particle != null)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
                ParticalEnder(particle, () =>
                {
                    particle.gameObject.SetActive(false);
                });
            }
        }
        
        private void ParticalEnder(ParticleSystem _particle, Action callback)
        {
            var sequence = DOTween.Sequence();
            var main = _particle.main;
            var duration = main.duration;
            sequence.AppendInterval(duration / 2).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        
        private void KillSequencesBeforeSequencePlay()
        {
            _sequence?.Kill();
            _lastSequence?.Kill();
        }
    }
}