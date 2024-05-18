using UnityEngine;

namespace Weapons
{
    public class GunsLoader : MonoBehaviour
    {
        [SerializeField] private GunsData _gunsData;
        [SerializeField] private GameObject _localWeapons;

        private void Awake()
        {
            GunsManager.Load(_gunsData.Guns);
            Gun[] guns = GunsManager.GameObjectToGuns(_localWeapons);
            GunsManager.CopyGunsValuesTo(ref guns);
        }
    }
}
