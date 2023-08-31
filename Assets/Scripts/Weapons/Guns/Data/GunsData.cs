using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "GunsData", menuName = "Weapons/Guns")]
    public class GunsData : ScriptableObject
    {
        public GunData[] Guns;
    }
}
