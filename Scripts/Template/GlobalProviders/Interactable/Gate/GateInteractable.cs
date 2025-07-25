using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Template.GlobalProviders.Combat;
using DG.Tweening;
using Fluxy;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Interactable.Gate
{
    public sealed class GateInteractable : BaseGateInteractable, IDamageableAction
    {
        #region Public Variables

        public TextMeshPro _amountText;

        private static readonly Dictionary<MathType, string> GateTypesDict = new()
        {
            { MathType.Divide, "÷" },
            { MathType.Multiply, "x" },
            { MathType.Subtract, "-" },
            { MathType.Add, "+" }
        };

        private static readonly Dictionary<MathType, Color> GateColorsDict = new()
        {
            { MathType.Divide, new Color(1, 0, 0.286f, 1)},
            { MathType.Subtract, new Color(1, 0, 0.286f, 1)},
            { MathType.Multiply, new Color(0, 0.973f, 1, 1)},
            { MathType.Add, new Color(0, 0.973f, 1, 1) }
        };

        #endregion

        #region Unity Methods

        private void Awake()
        {
            InitiateSetGateData();
            InitiateSetColorData();
        }

        #endregion

        #region Inherited Methods

        protected override void OnGateInteraction()
        {
            if(!isGateOpen) return;
            
            if (Container != null)
            {
                Container.targets = Array.Empty<FluxyTarget>();
                Destroy(Container.gameObject);
            }

            ShatterGlass();

            gameObject.SetActive(false);
        }

        #endregion

        #region Private Methods

        #region Initialization

        private void InitiateSetGateData()
        {
            StartCoroutine(DeferSetGateData());
        }

        private IEnumerator DeferSetGateData()
        {
            yield return null;
            UpdateGateAppearance();
        }

        [Button]
        private void ShatterGlass()
        {
            var root = GetRayFireRootObject();

            // Iterate through each child of the root GameObject
            foreach (Transform child in root.transform)
            {
                // Add a MeshCollider to the child if it doesn't already have one
                var collider = child.gameObject.GetComponent<MeshCollider>();
                if (collider == null)
                {
                    collider = child.gameObject.AddComponent<MeshCollider>();
                    collider.convex = true; // Set to true if you need a convex MeshCollider
                }

                DOVirtual.DelayedCall(shatterDisableTime, () =>
                {
                    child.transform.DOScale(Vector3.zero, 0.3f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => { Destroy(child.gameObject); });
                });
            }
        }

        private GameObject GetRayFireRootObject()
        {
            var parent = transform.parent;

            var root = parent.GetChild(parent.childCount - 1).gameObject;

            return root;
        }


        private void InitiateSetColorData()
        {
            StartCoroutine(DeferSetColorData());
        }

        private IEnumerator DeferSetColorData()
        {
            yield return null;
            UpdateGateAppearance();
        }

        #endregion

        #region Gate Data

        private void UpdateGateAppearance()
        {
            SetGateData();
            SetColorData(gateMeshRenderer);
            SetUpgradeText();
        }

        private void SetUpgradeText()
        {
            _upgradeText.text = gateInteractableData.UpgradeType.ToString().ToUpper();
        }

        private void SetGateData()
        {
            if (GateTypesDict.TryGetValue(gateInteractableData.mathType, out var symbol))
                _amountText.text = symbol + gateInteractableData.Amount;
            else
                throw new ArgumentOutOfRangeException();
        }

        #endregion

        #region Color Data

        private void SetColorData(MeshRenderer targetMeshRenderer)
        {
            const float normalizedAlpha = 80 / 255.0f; // Normalize alpha value to [0, 1]

            if (GateColorsDict.TryGetValue(gateInteractableData.mathType, out var color))
            {
                var renderer = targetMeshRenderer;

                var newColor = new Color(color.r, color.g, color.b, normalizedAlpha);

                // At runtime, create a new material instance
                var instanceMaterial = new Material(renderer.material);
                instanceMaterial.color = newColor;
                
                //Can be add IF_UNITY_EDITOR

                // Assign the new material instance to the renderer
                renderer.material = instanceMaterial;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(gateInteractableData.mathType),
                    $"No color found for mathType: {gateInteractableData.mathType}");
            }
        }


        private List<Rigidbody> GetGuardedGateObjects()
        {
            var totalBreakableObjects = guardedGateData.GuardedGateObjects.Count;
            var breakAmountPerStep = totalBreakableObjects / guardedGateData.MaxAmount;
            var breakableObjects = new List<Rigidbody>();

            var startIdx = guardedGateData.CurrentAmount * breakAmountPerStep;
            var endIdx = Mathf.Min(startIdx + breakAmountPerStep, totalBreakableObjects);

            for (var i = startIdx; i < endIdx; i++)
            {
                var selectedObject = guardedGateData.GuardedGateObjects[i];
                selectedObject.transform.SetParent(null);
                breakableObjects.Add(selectedObject.GetComponent<Rigidbody>());
            }

            return breakableObjects;
        }

        private void BreakGuardedGateObjects()
        {
            var breakableObjects = GetGuardedGateObjects();

            foreach (var breakableObject in breakableObjects)
            {
                breakableObject.isKinematic = false;
                breakableObject.useGravity = true;

                DOVirtual.DelayedCall(1, () =>
                {
                    breakableObject.transform.DOScale(Vector3.zero, 0.3f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => { Destroy(breakableObject.gameObject); });
                });
            }

            if (guardedGateData.CurrentAmount >= guardedGateData.MaxAmount) isGateOpen = true;
        }

        private bool IsGateOpen()
        {
            isGateOpen = guardedGateData.CurrentAmount == guardedGateData.MaxAmount;

            return isGateOpen;
        }

        private void IncrementalGateOpen()
        {
            guardedGateData.CurrentAmount += 1;
        }

        private void IncreaseGateAmount()
        {
            switch (gateInteractableData.mathType)
            {
                case MathType.Add:
                    gateInteractableData.Amount += 1;
                    break;
                case MathType.Subtract:
                    gateInteractableData.Amount -= 1;
                    break;
                case MathType.Multiply:
                    gateInteractableData.Amount += 1;
                    break;
                case MathType.Divide:
                    gateInteractableData.Amount -= 1;
                    break;
                default:
                    gateInteractableData.Amount -= 1;
                    break;
            }
        }

        [Button]
        private void GuardedGateOpenTask()
        {
            if (IsGateOpen())
            {
                IncreaseGateAmount();
                CheckGateInteractableDataAmountAndChangeMathType();
                UpdateGateAppearance();
            }
            else
            {
                BreakGuardedGateObjects();
                IncrementalGateOpen();
                CanInteract = IsGateOpen();
            }
        }

        #endregion

        #region Damage Handling

        private void CheckGateInteractableDataAmountAndChangeMathType()
        {
            if (gateInteractableData.Amount == 0 && gateInteractableData.mathType == MathType.Subtract)
            {
                gateInteractableData.mathType = MathType.Add;
                gateInteractableData.Amount = 1;
            }
            else if (gateInteractableData.Amount == 0 && gateInteractableData.mathType == MathType.Divide)
            {
                gateInteractableData.mathType = MathType.Multiply;
                gateInteractableData.Amount = 1;
            }
        }

        #endregion

        #endregion

        #region IDamageableAction Implementation

        public void Initialize(DamageableObject damageableObject)
        {
            // Implementation
        }

        public void TakeDamage(float damage)
        {
            if (_isGuardedGate)
            {
                GuardedGateOpenTask();
            }
            else
            {
                IncreaseGateAmount();
                CheckGateInteractableDataAmountAndChangeMathType();
                UpdateGateAppearance();
            }
        }

        public void HealthChanged(float currentHealth)
        {
            // Implementation
        }

        public void Death()
        {
            // Implementation
        }

        #endregion
    }
}