using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public class CursePoisonPoolInitializer : MonoBehaviour
    {
        [SerializeField] private CursePoisonPool _cursePoisonPool;
        [SerializeField] private ParticleSystemPool _poisonEffectPool;

        private void Start()
        {
            foreach (var projectile in _cursePoisonPool.Pool.Elements)
                projectile.EffectProvider = _poisonEffectPool.Pool;
            
            _cursePoisonPool.Pool.Expanded += OnExpanded;
        }

        private void OnExpanded(PoisonProjectile poisonProjectile)
        {
            poisonProjectile.EffectProvider = _poisonEffectPool.Pool;
        }

        private void OnDestroy()
        {
            _cursePoisonPool.Pool.Expanded -= OnExpanded;
        }
    }
}
