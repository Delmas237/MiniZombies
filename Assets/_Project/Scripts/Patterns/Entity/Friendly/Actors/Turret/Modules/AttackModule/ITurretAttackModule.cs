namespace Entity.Friendly.Turret
{
    public interface ITurretAttackModule : IModule
    {
        float Cooldown { get; }
    }
}
