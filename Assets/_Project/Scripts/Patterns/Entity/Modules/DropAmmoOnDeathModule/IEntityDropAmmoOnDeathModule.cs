using Weapons;

namespace Entity
{
    public interface IEntityDropAmmoOnDeathModule : IModule
    {
        IInstanceProvider<AmmoPack> AmmoProvider { get; set; }
    }
}
