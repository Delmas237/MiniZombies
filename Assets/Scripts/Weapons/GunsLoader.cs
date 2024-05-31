using ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons
{
    public class GunsLoader : MonoBehaviour
    {
        [SerializeField] private bool _loadData = true;
        [SerializeField] private List<GunInitializeData> _guns;

        private void Awake()
        {
            foreach (GunInitializeData gunInitializeData in _guns)
            {
                if (gunInitializeData != null && gunInitializeData.ShotPool != null)
                    gunInitializeData.Gun.ShotPool = gunInitializeData.ShotPool.Pool;
            }

            if (_loadData)
            {
                List<Gun> guns = _guns.Select(g => g.Gun).ToList();

                foreach (Gun gun in guns)
                    GunsDataSaver.LoadData(gun);
            }
        }
    }

    [Serializable]
    public class GunInitializeData
    {
        public Gun Gun;
        public ParticleSystemPool ShotPool;
    }
}
