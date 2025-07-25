using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class CarRotationLimiter : MonoBehaviour
    {
        // Adjustable parameters
        public float minRotationY = -45f; // Minimum Y rotation (left turn limit)
        public float maxRotationY = 0f;   // Maximum Y rotation (right turn limit)
        public float minRotationZ = -10f; // Minimum Z rotation (forward tilt limit)

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // Clamp rotation to stay within specified limits
            Vector3 clampedRotation = rb.rotation.eulerAngles;

            // Convert clampedRotation.y to [-180, 180] range
            if (clampedRotation.y > 180) clampedRotation.y -= 360;

            clampedRotation.y = Mathf.Clamp(clampedRotation.y, minRotationY, maxRotationY);

            // Apply the clamped rotation to the Rigidbody
            rb.rotation = Quaternion.Euler(clampedRotation);
        }
    }
}
