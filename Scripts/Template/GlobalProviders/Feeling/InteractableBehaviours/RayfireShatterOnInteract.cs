using _Game.Scripts.Template.GlobalProviders.Interactable;
using DG.Tweening;
using RayFire;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class RayfireShatterOnInteract : MonoBehaviour, IInteractableAction
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField, Range(0.5f, 0.8f)] private float _minForwardForce = 0.5f;
    [SerializeField, Range(0.5f, 0.8f)] private float _maxForwardForce = 0.8f;
    [SerializeField, Range(0.5f, 0.8f)] private float _minUpwardForce = 0.5f;
    [SerializeField, Range(0.5f, 0.8f)] private float _maxUpwardForce = 0.8f;
    [SerializeField, Range(0.5f, 0.8f)] private float _minSideForce = 0.5f;
    [SerializeField, Range(0.5f, 0.8f)] private float _maxSideForce = 0.8f;
    [SerializeField] private bool applyNegativeForce = true;

    private RayfireRigidRoot _rayfireRigidRoot;

    private void Awake()
    {
        _rayfireRigidRoot = GetComponent<RayfireRigidRoot>();
    }
    
    public void InteractableAction()
    {
        Shatter();
        ConfigureRigidbodies();
        HideTextMeshPro();
        ApplyRandomForces();
    }

    private void Shatter()
    {
        _rayfireRigidRoot.Initialize();
    }

    private void ConfigureRigidbodies()
    {
        foreach (var rb in _rigidbodies)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.gameObject.layer = LayerMask.NameToLayer("Brick");
            DOVirtual.DelayedCall(10f, () => rb.gameObject.SetActive(false));
        }
    }

    private void HideTextMeshPro()
    {
        _textMeshPro?.gameObject.SetActive(false);
    }
    
    private void ApplyRandomForces()
    {
        foreach (var rb in _rigidbodies)
        {
            float forwardForce = GetRandomForce(_minForwardForce, _maxForwardForce);
            float upwardForce = GetRandomForce(_minUpwardForce, _maxUpwardForce);
            float sideForce = Random.Range(-_maxSideForce, _maxSideForce);

            Vector3 force = CalculateForce(forwardForce, upwardForce, sideForce);
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    private float GetRandomForce(float min, float max)
    {
        float force = Random.Range(min, max);
        return applyNegativeForce && Random.value > 0.5f ? -force : force;
    }

    private Vector3 CalculateForce(float forward, float upward, float side)
    {
        return transform.forward * forward + transform.up * upward + transform.right * side;
    }

    [Button]
    private void SetRigidbodies()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
    }
}
