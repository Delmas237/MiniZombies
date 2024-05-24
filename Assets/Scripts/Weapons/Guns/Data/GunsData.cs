using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "GunsData", menuName = "Weapons/Guns")]
    public class GunsData : ScriptableObject
    {
        public List<GunData> Guns;
    }
}
