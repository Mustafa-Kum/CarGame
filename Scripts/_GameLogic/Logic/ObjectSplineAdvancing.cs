using _Game.Scripts.Managers.Core;
using Dreamteck.Splines;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class ObjectSplineAdvancing : BaseInputProvider
    {
        [SerializeField] private SplineFollower splineFollower;
        [SerializeField] private float swerveSpeed = 1f;
        [SerializeField] private float maxSwerveAmount;

        private Vector2 _startTouchPosition;
        private float _screenWidth;
        private float _swerveInput;
        private Vector2 lastTouchPosition;
        private float accumulatedSwerve;

        private void Start() => _screenWidth = Screen.width;

        private void LateUpdate()
        {
            UpdateSwerve();
        } 

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += OnLevelStarted;
            EventManager.InGameEvents.LevelSuccess += OnLevelEnded;
            EventManager.InGameEvents.LevelFail += OnLevelEnded;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= OnLevelStarted;
            EventManager.InGameEvents.LevelSuccess -= OnLevelEnded;
            EventManager.InGameEvents.LevelFail -= OnLevelEnded;
        }

        protected override void ClickedDown()
        {
            _startTouchPosition = Input.mousePosition;
            accumulatedSwerve = splineFollower.motion.offset.x; // mevcut konumu kaydet
        }

        protected override void ClickedHold()
        {
            Vector2 currentTouchPosition = Input.mousePosition;
            Vector2 deltaTouchPosition = currentTouchPosition - _startTouchPosition;
            _swerveInput = deltaTouchPosition.x / _screenWidth;
        }

        protected override void ClickedUp()
        {
            lastTouchPosition = Input.mousePosition;
        }

        private void OnLevelStarted()
        {
            splineFollower.follow = true;
        }

        private void OnLevelEnded()
        {
            splineFollower.follow = false;
        }

        private void UpdateSwerve()
        {
            if (!splineFollower.follow) return;

            Debug.Log("Swerve input: " + _swerveInput);

            float targetOffset = Mathf.Clamp(accumulatedSwerve + (_swerveInput * swerveSpeed), -maxSwerveAmount, maxSwerveAmount);
            splineFollower.motion.offset = new Vector2(targetOffset, splineFollower.motion.offset.y);
        }
    }
}