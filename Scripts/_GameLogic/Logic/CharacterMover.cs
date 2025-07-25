using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Input;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.InGame.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMover : InputProvider
    {
        public float speed = 5f;
        [SerializeField] private float swerveSpeed = 2f;
        [SerializeField] private float maxSwerveAmount = 1.5f;
        [SerializeField] private float clampXBorder = 2.5f;
        [SerializeField] private float maxRotationAngle = 15f;
        [SerializeField] private float pitchAngle = 5f;
        
        [SerializeField] private BaseSpeedUpgradeSO baseSpeedUpgradeSO;
        [SerializeField] private PlayerUpgradeData playerUpgradeData;


        private bool isMoving = false;
        private float swerveAmount;
        private Vector2 lastTouchPosition;
        private bool isTouching = false;
        private Rigidbody rb;
        private void Awake()
        {
            base.Initialize();
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.InGameEvents.LevelSuccess += StopCharacter;
            EventManager.InGameEvents.LevelFail += StopCharacter;
            EventManager.InGameEvents.LevelStart += MoveCharacter;
            EventManager.InGameEvents.LevelStart += CacheSpeedUpgradeData;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.InGameEvents.LevelSuccess -= StopCharacter;
            EventManager.InGameEvents.LevelFail -= StopCharacter;
            EventManager.InGameEvents.LevelStart -= MoveCharacter;
            EventManager.InGameEvents.LevelStart -= CacheSpeedUpgradeData;
        }
        
        private void CacheSpeedUpgradeData()
        {
            speed = baseSpeedUpgradeSO.GetValue(playerUpgradeData.GetUpgradeLevel(UpgradeType.BaseSpeedValue));
        }

        private void MoveCharacter()
        {
            isMoving = true;
        }

        private void StopCharacter()
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (isMoving)
            {
                Vector3 forwardMovement = Vector3.forward * speed * Time.fixedDeltaTime;
                Vector3 swerveMovement = Vector3.right * swerveAmount * swerveSpeed * Time.fixedDeltaTime;
                Vector3 newPosition = rb.position + forwardMovement + swerveMovement;

                newPosition.x = Mathf.Clamp(newPosition.x, -clampXBorder, clampXBorder);
                rb.MovePosition(newPosition);

                // Calculate the rotation based on the swerve amount
                float rotationAngle = maxRotationAngle * (swerveAmount / maxSwerveAmount) * 30;
                // Calculate the pitch angle based on speed
                float pitch = pitchAngle * Mathf.Sin(Time.time * speed);

                // Smooth Rotate
                Quaternion targetRotation = Quaternion.Euler(0, rotationAngle, pitch);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f));
            }

            if (isTouching)
            {
                Vector2 currentTouchPosition = Input.mousePosition;
                if (Input.touchCount > 0)
                {
                    currentTouchPosition = Input.GetTouch(0).position;
                }

                Vector2 delta = currentTouchPosition - lastTouchPosition;
                swerveAmount = Mathf.Lerp(swerveAmount, Mathf.Clamp(delta.x / Screen.width * maxSwerveAmount, -maxSwerveAmount, maxSwerveAmount), 0.1f);
                lastTouchPosition = currentTouchPosition;
            }
        }

        protected override void OnClickDown(ClickData clickData)
        {
            lastTouchPosition = Input.mousePosition;
            if (Input.touchCount > 0)
            {
                lastTouchPosition = Input.GetTouch(0).position;
            }
            isTouching = true;
        }

        protected override void OnClickHold(ClickData clickData)
        {
            // OnClickHold functionality is handled in FixedUpdate when isTouching is true
        }

        protected override void OnClickUp(ClickData clickData)
        {
            swerveAmount = 0f;
            isTouching = false;
        }
    }
}
