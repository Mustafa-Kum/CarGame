using _Game.Scripts.Helper.Services;
using Lean.Pool;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        #region Private Variables

        private ProjectileStructData projectileData;
        
        private CoroutineService coroutineService;
        private Coroutine translateCoroutine;
        private float distanceTraveled;
        
        #endregion

        #region Unity Methods

        private void Awake()
        {
            coroutineService = new CoroutineService(this);
        }

        private void OnEnable()
        {
            distanceTraveled = 0f;  
        }

        #endregion

        #region Public Methods

        public void OnSpawn()
        {
            distanceTraveled = 0f;
        }
        
        public void Initialize(ProjectileStructData data)
        {
            this.projectileData = data;
            translateCoroutine = coroutineService.StartUpdateRoutine(Translate, () => true);
        }

        #endregion

        #region Private Methods

        private void ResetProjectile()
        {
            distanceTraveled = 0f;
        }
    
        private void Translate()
        {
            var translationVector = transform.forward * (projectileData.speed * Time.deltaTime);

            distanceTraveled += projectileData.speed * Time.deltaTime;

            if (distanceTraveled >= projectileData.range)
            {
                distanceTraveled = 0;

                StopCoroutine(translateCoroutine);
                
                LeanPool.Despawn(gameObject);

                ResetProjectile();
                return;
            }

            transform.Translate(translationVector, Space.World);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponent<IDamageable>();
            
            if (damageable == null) return;
            
            damageable.TakeDamage(projectileData.damage);
            
            LeanPool.Despawn(gameObject);
        }

        #endregion
    }
}