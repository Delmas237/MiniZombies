using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Weapons/Gun")]
    public class GunData : ScriptableObject
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _distance = 4;
        [SerializeField] private int _consumption = 1;
        [Space(5)]
        [SerializeField] private GunType _type;
        [Space(5)]
        [SerializeField] private Sprite _icon;

        [Header("Audio")]
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private float _volume;
        [SerializeField] private float _pitch;

        [Header("Bullet Trail")]
        [SerializeField] private float _shapeAngle;

        public float Damage => _damage;
        public float Cooldown => _cooldown;
        public float Distance => _distance;
        public int Consumption => _consumption;
        public GunType Type => _type;
        public Sprite Icon => _icon;
        public AudioClip AudioClip => _audioClip;
        public float Volume => _volume;
        public float Pitch => _pitch;
        public float ShapeAngle => _shapeAngle;
    }
}
