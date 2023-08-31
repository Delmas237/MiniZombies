using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class PoolAmmoPack : MonoBehaviour, IPool<AmmoPack>
    {
        [SerializeField] private AmmoPack ammoPack;
        [SerializeField] private int amount = 5;
        [SerializeField] private bool autoExpand = true;
        [Space(10)]
        [SerializeField] private PoolAudioSource destroySoundPool;

        private PoolBase<AmmoPack> pool;

        private void Awake()
        {
            pool = new PoolBase<AmmoPack>(ammoPack, amount, transform)
            {
                AutoExpand = autoExpand
            };
        }

        public AmmoPack GetFreeElement()
        {
            AmmoPack ammoPack = pool.GetFreeElement();
            ammoPack.DestroySoundPool = destroySoundPool;
            return ammoPack;
        }
    }
}
