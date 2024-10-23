using ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class GunsLoader : MonoBehaviour
    {
        [SerializeField] private bool _loadData = true;
        [SerializeField] private BulletTrailPool _shotPool;

        [SerializeField] private List<Gun> _guns;

        private void Awake()
        {
            foreach (Gun gun in _guns)
            {
                gun.BulletPool = _shotPool.Pool;

                if (_loadData)
                    GunsDataSaver.LoadData(gun);
            }
        }
    }
}
