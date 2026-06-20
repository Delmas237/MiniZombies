using Entity.Hostile;
using UnityEngine;

namespace ObjectPool
{
    public abstract class EnemyPool : MonoBehaviour
    {
        public abstract IPool<ZombieEntity> Pool { get; }
    }
}
