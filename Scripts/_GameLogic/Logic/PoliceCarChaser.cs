using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.InGame.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PoliceCarChaser : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 10f;
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float baseMinDist = 5f; // Base minimum distance
        [SerializeField] private float randomRange = 2f; // Range for randomizing the minimum distance
        [SerializeField] private float decelerationDistance = 3f; // Distance over which to decelerate
        [SerializeField] private float hitDistance = 1.5f; // Distance within which the car tries to hit the target
        [SerializeField] private float distanceChangeInterval = 2f; // Interval for changing the stopping distance
        [SerializeField] private float randomXOffsetMin = -1f; // Minimum X offset
        [SerializeField] private float randomXOffsetMax = 1f; // Maximum X offset
        [SerializeField] private float xOffsetChangeInterval = 1f; // Interval for changing the X offset

        private bool isChasing = false;
        private Rigidbody rb;
        private float minDist;
        private float nextDistanceChangeTime;
        private float nextXOffsetChangeTime;
        private float currentSpeed = 0f;
        private float currentXOffset = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            minDist = GetRandomizedMinDist();
            nextDistanceChangeTime = Time.time + distanceChangeInterval;
            nextXOffsetChangeTime = Time.time + xOffsetChangeInterval;
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += StopChasing;
            EventManager.InGameEvents.LevelFail += StopChasing;
            EventManager.InGameEvents.LevelStart += StartChasing;
            EventManager.InGameEvents.PuzzleGameTransformTrigger += StopChasing;
            EventManager.InGameEvents.LevelStart += OutcastVehicleTransform;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= StopChasing;
            EventManager.InGameEvents.LevelFail -= StopChasing;
            EventManager.InGameEvents.LevelStart -= StartChasing;
            EventManager.InGameEvents.PuzzleGameTransformTrigger -= StopChasing;
            EventManager.InGameEvents.LevelStart -= OutcastVehicleTransform;
        }

        private void OutcastVehicleTransform()
        {
            EventManager.VehicleEvents.VehicleSpawned?.Invoke(transform);
        }
        
        private void StartChasing()
        {
            isChasing = true;
            minDist = GetRandomizedMinDist();
            nextDistanceChangeTime = Time.time + distanceChangeInterval;
            nextXOffsetChangeTime = Time.time + xOffsetChangeInterval;
            currentXOffset = GetRandomXOffset();
        }

        private void StopChasing()
        {
            isChasing = false;
            currentSpeed = 0f;
            rb.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (isChasing && target != null)
            {
                // Calculate the direction towards the target
                Vector3 directionToTarget = (target.position - rb.position).normalized;
                float distanceToTarget = Vector3.Distance(rb.position, target.position);

                // Dynamically adjust the stopping distance
                if (Time.time >= nextDistanceChangeTime)
                {
                    minDist = GetRandomizedMinDist();
                    nextDistanceChangeTime = Time.time + distanceChangeInterval;
                }

                // Dynamically adjust the X offset
                if (Time.time >= nextXOffsetChangeTime)
                {
                    currentXOffset = GetRandomXOffset();
                    nextXOffsetChangeTime = Time.time + xOffsetChangeInterval;
                }

                // Add the X offset to the target position
                Vector3 targetWithOffset = target.position + new Vector3(currentXOffset, 0, 0);
                directionToTarget = (targetWithOffset - rb.position).normalized;

                // Check if the chaser is within the minimum distance
                if (distanceToTarget > minDist)
                {
                    // Calculate speed based on acceleration
                    currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, maxSpeed);

                    // Calculate speed reduction based on proximity to the target
                    if (distanceToTarget < minDist + decelerationDistance)
                    {
                        currentSpeed = Mathf.Lerp(0, maxSpeed, (distanceToTarget - minDist) / decelerationDistance);
                    }

                    // Move the Rigidbody towards the target
                    Vector3 movement = directionToTarget * currentSpeed * Time.fixedDeltaTime;
                    Vector3 newPosition = rb.position + movement;

                    // Smoothly interpolate position
                    rb.MovePosition(Vector3.Lerp(rb.position, newPosition, Time.fixedDeltaTime * currentSpeed));

                    // Rotate to face the target
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                }
                else if (distanceToTarget <= hitDistance)
                {
                    // Simulate a hit by moving forward slightly
                    Vector3 hitMovement = directionToTarget * currentSpeed * Time.fixedDeltaTime;
                    rb.MovePosition(rb.position + hitMovement);

                    // Apply DOTween shake to player to simulate a hit
                    DOTween.Shake(() => target.position, x => target.position = x, 0.5f, 0.5f, 10, 90, false);

                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                }
                else
                {
                    // Gradually slow down when within the minimum distance to reduce jittering
                    currentSpeed = Mathf.Max(currentSpeed - acceleration * Time.fixedDeltaTime, 0f);
                    rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * currentSpeed);
                }
            }
        }

        private float GetRandomizedMinDist()
        {
            return baseMinDist + Random.Range(-randomRange, randomRange);
        }

        private float GetRandomXOffset()
        {
            return Random.Range(randomXOffsetMin, randomXOffsetMax);
        }
    }
}
