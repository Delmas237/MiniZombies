using Entity.Friendly.Turret;
using EventBusLib;
using ObjectPool;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class TurretFactory : MonoBehaviour, IFactory<TurretEntity>, IInstanceProvider<TurretEntity>
    {
        [SerializeField] private TurretPool _turretPool;
        [SerializeField] private BulletTrailPool _bulletTrailPool;
        private IPool<TurretEntity> _pool;
        private TurretEntity[] _prefabs;

        public IPool<TurretEntity> Pool => _pool;
        public TurretEntity[] Prefabs => _prefabs;

        private void Start()
        {
            _pool = _turretPool.Pool;
            _prefabs = _turretPool.Pool.Prefabs;

            foreach (TurretEntity turret in Pool.Elements)
                InitializeTurret(turret);

            _pool.Expanded += InitializeTurret;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent gameOverEvent)
        {
            _pool.Expanded -= InitializeTurret;
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        }

        private void InitializeTurret(TurretEntity turret)
        {
            foreach (Gun gun in turret.WeaponModule.Guns)
            {
                gun.BulletPool = _bulletTrailPool.Pool;
            }
        }

        public TurretEntity GetInstance()
        {
            TurretEntity instance = _pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        public void ReconstructToDefault(TurretEntity prefab)
        {
            prefab.HealthModule.Increase(prefab.HealthModule.MaxHealth);
        }
        public void Construct(TurretEntity prefab)
        {
            prefab.AttackModule.Install();
        }

        public TurretEntity NewInstance() => Object.Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
