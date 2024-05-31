using EnemyLib;
using UnityEngine;

namespace ObjectPool
{
    public abstract class EnemyPool : MonoBehaviour
    {
        public abstract IPool<ZombieContainer> Pool { get; }
    }
}
