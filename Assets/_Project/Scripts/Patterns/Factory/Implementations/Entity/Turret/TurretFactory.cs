using EntityLib;
using EntityLib.Friendly.Turret;
using EventBusLib;
using ObjectPool;
using UnityEngine;
using Weapons;

namespace Factory
{
    public class TurretFactory : MonoBehaviour, IFactory<Entity>, IInstanceProvider<Entity>
    {
        [SerializeField] private EntityPool _turretPool;
        [SerializeField] private BulletTrailPool _bulletTrailPool;
        private IPool<Entity> _pool;
        private Entity[] _prefabs;

        public IPool<Entity> Pool => _pool;
        public Entity[] Prefabs => _prefabs;

        private void Start()
        {
            _pool = _turretPool.Pool;
            _prefabs = _turretPool.Pool.Prefabs;

            foreach (Entity turret in Pool.Elements)
                InitializeTurret(turret);

            _pool.Expanded += InitializeTurret;
            EventBus.Subscribe<GameExitEvent>(Unsubscribe);
        }
        private void Unsubscribe(GameExitEvent gameOverEvent)
        {
            _pool.Expanded -= InitializeTurret;
            EventBus.Unsubscribe<GameExitEvent>(Unsubscribe);
        }

        private void InitializeTurret(Entity turret)
        {
            var weaponModule = turret.GetModule<IEntityWeaponModule>();
            foreach (Gun gun in weaponModule.Guns)
            {
                gun.BulletPool = _bulletTrailPool.Pool;
            }
        }

        public Entity GetInstance()
        {
            Entity instance = _pool.GetInstance();

            ReconstructToDefault(instance);
            Construct(instance);

            return instance;
        }

        public void ReconstructToDefault(Entity turret)
        {
            turret.SetAllModulesInitialState();
            turret.HealthModule.Increase(turret.HealthModule.MaxHealth);
        }
        public void Construct(Entity turret)
        {
            var installModule = turret.GetModule<ITurretInstallModule>();
            installModule.Install();
        }

        public Entity NewInstance() => Object.Instantiate(Prefabs[Random.Range(0, Prefabs.Length)]);
    }
}
