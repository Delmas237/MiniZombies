using UnityEngine;

namespace Weapons
{
    public class GunsLoader : MonoBehaviour
    {
        [SerializeField] private GunsData gunsData;
        [SerializeField] private GameObject localWeapons;

        private void Awake()
        {
            GunsManager.Load(gunsData.Guns);
            Gun[] guns = GunsManager.GameObjectToGuns(localWeapons);
            GunsManager.CopyGunsValuesTo(ref guns);
        }
    }
}
