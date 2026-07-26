using Entity;
using Entity.Friendly.Turret;
using EventBusLib;
using ObjectPool;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class TurretFactory : MonoBehaviour, IFactory<EntityBase>, IInstanceProvider<EntityBase>
    {
        [SerializeField] private EntityPool _turretPool;
        [SerializeField] private BulletTrailPool _bulletTrailPool;
        private IPool<EntityBase> _pool;
        private EntityBase[] _prefabs;

        public IPool<EntityBase> Pool => _pool;
        public EntityBase[] Prefabs => _prefabs;

        private void Start()
        {
            _pool = _turretPool.Pool;
            _prefabs = _turretPool.Pool.Prefabs;

            foreach (EntityBase turret in Pool.Elements)
                InitializeTurret(turret);

            _pool.Expanded += InitializeTurret;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent gameOverEvent)
        {
            _pool.Expanded -= InitializeTurret;
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        }

        private void InitializeTurret(EntityBase turret)
        {
            var weaponModule = turret.GetModule<IEntityWeaponModule>();
            foreach (Gun gun in weaponModule.Guns)
            {
                gun.BulletPool = _bulletTrailPool.Pool;
            }
        }

        public EntityBase GetInstance()
        {
            EntityBase instance = _pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        public void ReconstructToDefault(EntityBase turret)
        {
            turret.SetAllModulesInitialState();
            turret.HealthModule.Increase(turret.HealthModule.MaxHealth);
        }
        public void Construct(EntityBase turret)
        {
            var installModule = turret.GetModule<ITurretInstallModule>();
            installModule.Install();
        }

        public EntityBase NewInstance() => Object.Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
