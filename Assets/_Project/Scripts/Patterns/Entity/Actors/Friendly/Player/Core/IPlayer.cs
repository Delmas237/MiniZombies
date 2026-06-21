using Entity;

namespace Player
{
    public interface IPlayer : IFriendly
    {
        IPlayerCurrencyModule CurrencyModule { get; }
        IPlayerWeaponModule WeaponModule { get; }
        IPlayerMovementModule MovementModule { get; }
    }
}
