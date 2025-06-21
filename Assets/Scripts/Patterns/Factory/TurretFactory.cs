using EventBusLib;
using ObjectPool;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class TurretFactory : MonoBehaviour, IFactory<TurretContainer>, IInstanceProvider<TurretContainer>
    {
        [SerializeField] private TurretPool _turretPool;
        [SerializeField] private BulletTrailPool _bulletTrailPool;
        private IPool<TurretContainer> _pool;
        private TurretContainer[] _prefabs;

        public IPool<TurretContainer> Pool => _pool;
        public TurretContainer[] Prefabs => _prefabs;

        private void Start()
        {
            _pool = _turretPool.Pool;
            _prefabs = _turretPool.Pool.Prefabs;

            foreach (TurretContainer turret in Pool.Elements)
                InitializeTurret(turret);

            _pool.Expanded += InitializeTurret;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent gameOverEvent)
        {
            _pool.Expanded -= InitializeTurret;
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        }

        private void InitializeTurret(TurretContainer turret)
        {
            foreach (Gun gun in turret.WeaponsModule.Guns)
            {
                gun.BulletPool = _bulletTrailPool.Pool;
            }
        }

        public TurretContainer GetInstance()
        {
            TurretContainer instance = _pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        public void ReconstructToDefault(TurretContainer prefab)
        {
            prefab.HealthModule.Increase(prefab.HealthModule.MaxHealth);
        }
        public void Construct(TurretContainer prefab)
        {
            prefab.AnimationModule.InstallAnim();
        }

        public TurretContainer NewInstance() => Object.Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
