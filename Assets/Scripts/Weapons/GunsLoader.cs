using ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons
{
    public class GunsLoader : MonoBehaviour
    {
        [SerializeField] private GunsData _gunsData;
        [SerializeField] private List<GunInitializeData> _guns;

        private void Awake()
        {
            GunsDataSaver.Load(_gunsData.Guns);

            foreach (GunInitializeData gunInitializeData in _guns)
                gunInitializeData.Gun.ShotPool = gunInitializeData.ShotPool;

            List<Gun> guns = _guns.Select(g => g.Gun).ToList();
            GunsDataSaver.CopyToGunsData(guns);
        }
    }

    [Serializable]
    public class GunInitializeData
    {
        public Gun Gun;
        public PoolParticleSystem ShotPool;
    }
}
