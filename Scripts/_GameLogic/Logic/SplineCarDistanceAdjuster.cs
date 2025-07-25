using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using Dreamteck.Splines;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class SplineCarDistanceAdjuster : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private float desiredDistance = 5f; // Desired distance to maintain from the target
        [SerializeField] private float adjustmentSpeed = 0.1f; // Factor for how quickly the speed adjusts
        
        private CoroutineService _coroutineService;

        private void Awake()
        {
            _coroutineService = new CoroutineService(this);
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += OnLevelStarted;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= OnLevelStarted;
        }
        
        private void OnLevelStarted()
        {
            _coroutineService.StartUpdateRoutine( AdjustFollowerSpeed , () => true);
        }


        private void AdjustFollowerSpeed()
        {
            float currentDistance = Vector3.Distance(splineFollower.transform.position, targetTransform.position);

            // Calculate the difference between current distance and desired distance
            float distanceDifference = currentDistance - desiredDistance;
            
            // Adjust the follower's speed based on the distance difference
            if (distanceDifference > 0) // Too far from target
            {
                splineFollower.followSpeed -= adjustmentSpeed * Time.deltaTime;
            }
            else if (distanceDifference < 0) // Too close to target
            {
                splineFollower.followSpeed += adjustmentSpeed * Time.deltaTime;
            }

            // Ensure the speed is non-negative
            if (splineFollower.followSpeed < 0)
            {
                splineFollower.followSpeed = 0;
            }
        }
    }
}