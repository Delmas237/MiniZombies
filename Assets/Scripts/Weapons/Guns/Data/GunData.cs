using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Weapons/Gun")]
    public class GunData : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; set; }

        [field: SerializeField] public float Cooldown { get; set; }
        [field: SerializeField] public float Distance { get; set; } = 4;
        [field: SerializeField] public int Consumption { get; set; } = 1;
        [field: Space(5)]
        [field: SerializeField] public GunType Type { get; private set; }
    }
}
