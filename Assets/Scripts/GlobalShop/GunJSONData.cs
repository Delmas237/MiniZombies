namespace Weapons
{
    public class GunJSONData
    {
        public GunType Type;
        public float Damage;
        public float Cooldown;
        public float Distance;

        public GunJSONData(GunType type, float damage, float cooldown, float distance)
        {
            Type = type;
            Damage = damage;
            Cooldown = cooldown;
            Distance = distance;
        }
    }
}
