using DG.Tweening;
using UnityEngine;
namespace _Game.Scripts.Managers.Core.CharacterManager
{
    [RequireComponent(typeof(Animator))]


    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField]  Animator _animator;
         
        
        private void Awake()
        {
            if (_animator==null)
            {
                _animator= GetComponent<Animator>();
            }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        private void SubscribeEvents()
        {
            EventManager.InGameEvents.LevelSuccess += DanceState;
            EventManager.InGameEvents.LevelFail += IdleState;
            EventManager.InGameEvents.LevelStart += RunState;
            EventManager.InGameEvents.LevelLoaded += IdleState;
        }

        private void UnsubscribeEvents()
        {
            EventManager.InGameEvents.LevelSuccess -= DanceState;
            EventManager.InGameEvents.LevelFail -= IdleState;
            EventManager.InGameEvents.LevelStart -= RunState;
            EventManager.InGameEvents.LevelLoaded -= IdleState;
        }

        private void Start()
        {
            IdleState();
        }
 
        private void RunState()
        {

            _animator.SetTrigger("Run");
        }

        private void IdleState(GameObject level)
        {
            _animator.SetTrigger("Idle");
        }

        private void IdleState()
        {
            _animator.SetTrigger("Idle");
            transform.LookAt(transform.parent);
        }

        private void DanceState()
        {
            _animator.SetTrigger("Dance");
        }

        // New method to trigger Run1 animation
        private void Run1(float param1, float param2)
        {
            _animator.SetTrigger("Run");
        }
    }
}

