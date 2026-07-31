using Weapons;

namespace EntityLib
{
    public interface IEntityDropAmmoOnDeathModule : IModule
    {
        IInstanceProvider<AmmoPack> AmmoProvider { get; set; }
    }
}
