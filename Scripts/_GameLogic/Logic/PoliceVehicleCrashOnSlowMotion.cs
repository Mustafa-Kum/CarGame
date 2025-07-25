using System.Collections.Generic;
using _Game.Scripts.InGame.Controllers;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic
{
    public class PoliceVehicleCrashOnSlowMotion : MonoBehaviour
    {
        public List<Transform> policeVehicles;
        public int PoliceVehicleEffectCount;
        public ParticleSystem crashEffectParticle;

        private void OnEnable()
        {
            EventManager.InteractableEvents.CopCrashableObstacleInteract += SetPoliceVehicleCrash;
            EventManager.VehicleEvents.VehicleSpawned += OnVehicleSpawned;
        }

        private void OnDisable()
        {
            EventManager.InteractableEvents.CopCrashableObstacleInteract -= SetPoliceVehicleCrash;
            EventManager.VehicleEvents.VehicleSpawned -= OnVehicleSpawned;
        }

        private List<Transform> GetRandomPoliceVehicleRbs()
        {
            List<Transform> selectedPoliceVehicles = new List<Transform>();
            for (int i = 0; i < PoliceVehicleEffectCount; i++)
            {
                Transform randomVehicle = policeVehicles[Random.Range(0, policeVehicles.Count)];
                selectedPoliceVehicles.Add(randomVehicle);
                policeVehicles.Remove(randomVehicle);
            }
            return selectedPoliceVehicles;
        }
        
        private void OnVehicleSpawned(Transform arg0)
        {
            policeVehicles.Add(arg0);
        }

        private void SetPoliceVehicleCrash()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                List<Transform> selectedPoliceVehicles = GetRandomPoliceVehicleRbs();
                foreach (var vehicle in selectedPoliceVehicles)
                {
                    if (vehicle.TryGetComponent(out Rigidbody vehicleRb) &&
                        vehicle.TryGetComponent(out PoliceCarChaser policeCarChaser))
                    {
                        var particle = Instantiate(crashEffectParticle, vehicle.position, Quaternion.identity);
                        Destroy(particle.gameObject, 2f);

                        policeCarChaser.enabled = false;
                        vehicleRb.constraints = RigidbodyConstraints.None;

                        // Apply an upward force
                        vehicleRb.AddForce(Vector3.up * 10f, ForceMode.Impulse);

                        // Apply a random torque to create a flipping effect
                        Vector3 randomTorque = new Vector3(
                            Random.Range(-1f, 1f),
                            Random.Range(-1f, 1f),
                            Random.Range(-1f, 1f)
                        );

                        vehicleRb.AddTorque(randomTorque * 10f, ForceMode.Impulse);
                    }
                }
            });
        }
    }
}
