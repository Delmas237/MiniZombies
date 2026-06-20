using Entity;

namespace Player
{
    public interface IPlayer : IFriendly
    {
        IPlayerCurrencyModule CurrencyModule { get; }
        IPlayerWeaponModule WeaponsModule { get; }
        IPlayerMovementModule MovementModule { get; }
    }
}
