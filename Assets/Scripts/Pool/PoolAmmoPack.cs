using UnityEngine;
using Weapons;

namespace ObjectPool
{
    public class PoolAmmoPack : MonoBehaviour, IPool<AmmoPack>
    {
        [SerializeField] private AmmoPack _ammoPack;
        [SerializeField] private int _amount = 5;
        [SerializeField] private bool _autoExpand = true;
        [Space(10)]
        [SerializeField] private PoolAudioSource _destroySoundPool;

        private PoolBase<AmmoPack> _pool;

        private void Awake()
        {
            _pool = new PoolBase<AmmoPack>(_ammoPack, _amount, transform)
            {
                AutoExpand = _autoExpand
            };
        }

        public AmmoPack GetFreeElement()
        {
            AmmoPack ammoPack = _pool.GetFreeElement();
            ammoPack.Intialize(_destroySoundPool);
            return ammoPack;
        }
    }
}
