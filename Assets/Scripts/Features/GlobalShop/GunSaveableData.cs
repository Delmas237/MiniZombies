namespace Weapons
{
    public class GunSaveableData
    {
        public GunType Type;
        public float Damage;
        public float Cooldown;
        public float Distance;

        public GunSaveableData(GunType type, float damage, float cooldown, float distance)
        {
            Type = type;
            Damage = damage;
            Cooldown = cooldown;
            Distance = distance;
        }

        public GunSaveableData(GunData gunData) : this(gunData.Type, gunData.Damage, gunData.Cooldown, gunData.Distance) { }
    }
}
