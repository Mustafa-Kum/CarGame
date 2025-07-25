using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.Template.GlobalProviders.Interactable;
using _Game.Scripts.Template.GlobalProviders.Interactable.Gate;
using _Game.Scripts.Template.GlobalProviders.Upgrade;
using Fluxy;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Combat
{
    public sealed class Shooter : ShooterProvider
    {
        #region Public Variables
        private CoroutineService CoroutineService { get; set; }
        
        [SerializeField] private LeanGameObjectPool bulletPool;
        
        [SerializeField] private PlayerUpgradeData _playerUpgradeDataSO;
        
        private ProjectileStructData _projectileStructData = new ProjectileStructData();
        
        [SerializeField] private WeaponStructData _weaponStructData = new WeaponStructData();

        private Transform projectileHolder;
        
        #endregion

        #region Private Variables

        private Coroutine _fireCoroutine;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            CoroutineService = new CoroutineService(this);
            
            InitWeaponData();
        }

        internal void OnEnable() => Subscribe();

        internal void OnDisable() => UnSubscribe();

        #endregion

        #region Private Methods

        private void Subscribe()
        {
            EventManager.InGameEvents.LevelStart += StartFire;
            EventManager.InGameEvents.LevelSuccess += StopFire;
            EventManager.InGameEvents.LevelFail += StopFire;
            EventManager.InGameEvents.LevelLoaded += StopFireOnLevelLoad;
            
            EventManager.InGameEvents.LevelSuccess += DespawnAllPooledProjectileInstances;
            EventManager.InGameEvents.LevelFail += DespawnAllPooledProjectileInstances;
            
            EventManager.InteractableEvents.GateInteract += GateInteractRestartFire;
            EventManager.InteractableEvents.BoostInteract += BoostInteractRestartFire;
        }
        
        private void UnSubscribe()
        {
            EventManager.InGameEvents.LevelStart -= StartFire;
            EventManager.InGameEvents.LevelSuccess -= StopFire;
            EventManager.InGameEvents.LevelFail -= StopFire;
            EventManager.InGameEvents.LevelLoaded -= StopFireOnLevelLoad;
            
            EventManager.InGameEvents.LevelSuccess -= DespawnAllPooledProjectileInstances;
            EventManager.InGameEvents.LevelFail -= DespawnAllPooledProjectileInstances;
            
            EventManager.InteractableEvents.GateInteract -= GateInteractRestartFire;
            EventManager.InteractableEvents.BoostInteract -= BoostInteractRestartFire;
            
            StopFire();
        }
        
        private void StartFire()
        {
            InitProjectileData();
            _fireCoroutine = CoroutineService.StartIntervalRoutine(Shoot, fireRateUpgradable.GetValue(_playerUpgradeDataSO.GetUpgradeLevel(fireRateUpgradable.upgradeType)), ()=> true);
            
            InvokeFluidTargetOnShoot();
        }
        
        private void Shoot()
        {
            InitProjectile();
            
            EventManager.ShootableEvents.OnWeaponShootUpdate?.Invoke();
        }
        
        private void StopFire()
        {
            CoroutineService.Stop(_fireCoroutine);
        }
        
        private void StopFireOnLevelLoad(GameObject level)
        {
            StopFire();
        }
        
        private void OnWeaponUpgradeStopFire(int weaponLevel)
        {
            StopFire();
        }
        
        private void OnWeaponUpgradeStartFire(int weaponLevel)
        {
            StartFire();
        }

        private void GateInteractRestartFire(GateInteractableData data)
        {
            StopFire();
            StartFire();
        }
        
        private void OnSauceSliderFilledRestartFire(bool isSliderFilled)
        {
            StopFire();
            StartFire();
        }
        
        [Button]
        private void InvokeFluidTargetOnShoot()
        {
            if (_weaponStructData.target == null) return;
            EventManager.ShootableEvents.BulletFluxyFluidOnShoot?.Invoke(_weaponStructData.target);
        }
        
        private void InitWeaponData()
        {
            if (_weaponStructData.bulletObject == null)
            {
                var bullet = CreateProjectileInstancePrimitive();
                _weaponStructData.bulletObject = bullet;
                bullet.SetActive(false);
            }
            
            if (_weaponStructData.weaponObject == null)
                _weaponStructData.weaponObject = gameObject;
            
            if (_weaponStructData.muzzle == null)
                _weaponStructData.muzzle = transform.GetChild(0);
            
            SetLeanPoolInstance();
        }
        
        private void InitProjectileData()
        {
            _projectileStructData = new ProjectileStructData
            {
                speed = _speedUpgradable.GetValue(_playerUpgradeDataSO.GetUpgradeLevel(_speedUpgradable.upgradeType)),
                range = _rangeUpgradable.GetValue(_playerUpgradeDataSO.GetUpgradeLevel(_rangeUpgradable.upgradeType) / 2),
                damage = _damageUpgradable.GetValue(_playerUpgradeDataSO.GetUpgradeLevel(_damageUpgradable.upgradeType)),
            };
        }

        #region Bullet Initiliaze

        private GameObject CreateProjectileInstancePrimitive()
        {
            var primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            primitive.name = "Projectile Clone";
            primitive.transform.localScale = Vector3.one * 0.5f;
            primitive.GetComponent<Collider>().isTrigger = true;
            primitive.AddComponent<Rigidbody>().isKinematic = true;
            primitive.AddComponent<Projectile>();
            return primitive;
        }

        private GameObject SpawnPooledProjectileInstance()
        {
            var _projectile = LeanPool.Spawn(
                _weaponStructData.bulletObject, 
                _weaponStructData.muzzle.position, 
                transform.rotation);
            
            return _projectile;
        }

        private void GetProjectileHolder(Transform _holder)
        {
            projectileHolder = _holder;
        }
        
        private void DespawnAllPooledProjectileInstances()
        {
            LeanPool.DespawnAll();
        }

        #endregion
        
        private void SetLeanPoolInstance()
        {
            if (bulletPool == null) return;
            bulletPool.Prefab = _weaponStructData.bulletObject;
        }
        
        private void BoostInteractRestartFire(BoostInteractableData arg0)
        {
            StopFire();
            StartFire();
        }

        private void InitProjectile()
        {
            var projectile = SpawnPooledProjectileInstance().GetComponent<Projectile>();
         
            _weaponStructData.target = projectile.GetComponent<FluxyTarget>();
            
            projectile.OnSpawn();
            
            projectile.Initialize(_projectileStructData);
        }
        
        #endregion
    }
}
