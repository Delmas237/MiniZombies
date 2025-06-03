namespace Weapons
{
    public class GunSavableData
    {
        public GunType Type;
        public float Damage;
        public float Cooldown;
        public float Distance;

        public GunSavableData(GunType type, float damage, float cooldown, float distance)
        {
            Type = type;
            Damage = damage;
            Cooldown = cooldown;
            Distance = distance;
        }

        public GunSavableData(GunData gunData) : this(gunData.Type, gunData.Damage, gunData.Cooldown, gunData.Distance) { }
    }
}
