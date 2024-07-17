using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Weapons/Gun")]
    public class GunData : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }

        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public float Distance { get; private set; } = 4;
        [field: SerializeField] public int Consumption { get; private set; } = 1;
        [field: Space(5)]
        [field: SerializeField] public GunType Type { get; private set; }
        [field: Space(5)]
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: Header("Audio")]
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public float Volume { get; private set; }
        [field: SerializeField] public float Pitch { get; private set; }
        
        [field: Header("Bullet Trail")]
        [field: SerializeField] public float ShapeAngle { get; private set; }
    }
}
