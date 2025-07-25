using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts._GameLogic.Logic
{
    public class PedVehicleCrashOnInteract : MonoBehaviour, IInteractableAction
    {
        [SerializeField] private ParticleSystem crashEffectParticle;
        [SerializeField] private Transform vehicle;
        [SerializeField] private Rigidbody vehicleRb;
        [SerializeField] private Collider mainCollider;
        [SerializeField] private float vehicleSpeed;
        [SerializeField] private float crashForce;
        
        private bool isMoving;

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += SetPedVehicleSpeed;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= SetPedVehicleSpeed;
        }

        private void Update()
        {
            if (isMoving) vehicle.Translate(Vector3.forward * (vehicleSpeed * Time.deltaTime));
        }

        public void InteractableAction()
        {
            SetPedVehicleCrash();
            
            EventManager.InteractableEvents.SpeedReduceInteract.Invoke();
        }

        private void SetPedVehicleSpeed()
        {
            isMoving = true;
        }

        private void SetPedVehicleCrash()
        {
            isMoving = false;
            mainCollider.enabled = false;
            
            var particle = Instantiate(crashEffectParticle, vehicle.position, Quaternion.identity);
            Destroy(particle.gameObject, 2f);

            vehicleRb.constraints = RigidbodyConstraints.None;

            vehicleRb.AddForce(Vector3.up * crashForce * 50 * Time.deltaTime, ForceMode.Impulse);
            vehicleRb.AddForce(Vector3.forward * crashForce * 50 * Time.deltaTime, ForceMode.Impulse);

            var randomTorque = new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );

            vehicleRb.AddTorque(randomTorque * 10, ForceMode.Impulse);


            DOVirtual.DelayedCall(3, () =>
            {
                Destroy(vehicle.gameObject);
            });
        }
    }
}