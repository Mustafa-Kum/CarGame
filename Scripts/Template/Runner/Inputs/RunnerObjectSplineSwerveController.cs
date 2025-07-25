using System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.InGame.ReferenceHolder;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

namespace _Game.Scripts.Template.Runner.Inputs
{
    public class RunnerObjectSplineSwerveController : MonoBehaviour
    {
        #region Private Variables

        private CoroutineService _coroutineService;

        #endregion

        #region Input Variables

        [SerializeField] private InputDataContainer.TransformSettings _transformSettings;
        [SerializeField] private InputDataContainer.InputSettings _inputSettings;
        [SerializeField] private InputDataContainer.InputConstants _inputConstants;
        [SerializeField] private InputDataContainer.SplineSettings _splineSettings;
        private InputDataContainer.InputInternalState _inputInternalState;

        private Vector3 currentMousePosition;
        private Vector3 mouseDelta;

        private Tween _weaponTiltTween;

        private Rigidbody _rigidbody;
        private SplineFollower _splineFollower;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _coroutineService = new CoroutineService(this);
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.useGravity = true;
            }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            StopInput();
        }

        #endregion

        #region Subscribe/Unsubscribe Events

        private void SubscribeEvents()
        {
            EventManager.InGameEvents.LevelLoaded += GetDefaults;
            EventManager.InGameEvents.LevelStart += StartInput;
            EventManager.InGameEvents.LevelSuccess += StopInput;
            EventManager.InGameEvents.LevelFail += StopInput;
        }

        private void UnsubscribeEvents()
        {
            EventManager.InGameEvents.LevelLoaded -= GetDefaults;
            EventManager.InGameEvents.LevelStart -= StartInput;
            EventManager.InGameEvents.LevelSuccess -= StopInput;
            EventManager.InGameEvents.LevelFail -= StopInput;
        }

        #endregion

        #region Private Methods

        private void GetDefaults(GameObject go)
        {
            if (IsSplineModeActive)
                GetSplineDefaults(go);
            else
                GetTransformDefaults();
        }

        private void GetTransformDefaults()
        {
            if (_transformSettings._targetTransform == null)
            {
                _transformSettings._targetTransform = transform;
            }
        }

        private void GetSplineDefaults(GameObject go)
        {
            _splineFollower = GetComponent<SplineFollower>();
            // _splineFollower.spline = go.GetComponent<LevelReferenceHolder>().SplineComputer;
            _splineFollower.motion.offset = new Vector2(0f, _splineFollower.motion.offset.y);
        }

        private void StartInput()
        {
            _coroutineService.StartLateUpdateRoutine(InputUpdate, () => true);
        }

        private void StopInput()
        {
            _coroutineService.StopAll();
        }

        private bool IsSplineModeActive => _transformSettings.splineMode;

        private void InputUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                currentMousePosition = Input.mousePosition;

                _inputInternalState.Sensitivity = _inputSettings.BaseSensitivity * (Screen.width / 1080f);
                mouseDelta = currentMousePosition - new Vector3(currentMousePosition.x - Input.GetAxis("Mouse X"),
                    currentMousePosition.y,
                    currentMousePosition.z);

                var horizontalInput = mouseDelta.x * _inputInternalState.Sensitivity;
                HandleHorizontalInput(horizontalInput);
            }
            else
            {
                _inputInternalState.CurrentHorizontalInput = 0;
                mouseDelta = Vector3.zero;
            }
        }

        private void HandleHorizontalInput(float horizontalInput)
        {
            _inputInternalState.LastHorizontalInput = horizontalInput;
            HandleHorizontalInputWithDamping(_inputInternalState.LastHorizontalInput);
        }

        private void HandleHorizontalInputWithDamping(float newHorizontalInput)
        {
            _inputInternalState.CurrentHorizontalInput = Mathf.Lerp(_inputInternalState.CurrentHorizontalInput, newHorizontalInput, _inputConstants.InputLag);

            if (_transformSettings.splineMode) HandleSplineModeActive();
            else HandleSplineModeInactive();
        }

        private void HandleSplineModeActive()
        {
            var splineMotion = _splineFollower.motion;

            var newOffsetX = Mathf.Clamp(splineMotion.offset.x + _inputInternalState.CurrentHorizontalInput, _inputSettings.minRangeX, _inputSettings.maxRangeX);

            var smoothedOffsetX = Mathf.Lerp(splineMotion.offset.x, newOffsetX, _inputConstants.Damping * Time.deltaTime);

            splineMotion.offset = new Vector2(smoothedOffsetX, splineMotion.offset.y);
            _splineFollower.motion.offset = splineMotion.offset;

            ApplyHorizontalForce(smoothedOffsetX);
        }

        private void HandleSplineModeInactive()
        {
            var position = _transformSettings._targetTransform.position;

            var clampedX = Mathf.Clamp(position.x + _inputInternalState.CurrentHorizontalInput, _inputSettings.minRangeX, _inputSettings.maxRangeX);
            var newX = Mathf.Lerp(position.x, clampedX, _inputConstants.Damping * Time.deltaTime);

            _transformSettings._targetTransform.position = new Vector3(newX, position.y, position.z);

            ApplyHorizontalForce(newX - position.x);
        }

        private void ApplyHorizontalForce(float horizontalOffset)
        {
            if (_rigidbody != null)
            {
                Vector3 horizontalForce = new Vector3(horizontalOffset, 0, 0);
                _rigidbody.AddForce(horizontalForce, ForceMode.VelocityChange);
            }
        }

        #endregion
    }
}
