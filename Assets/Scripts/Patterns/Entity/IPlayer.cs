using PlayerLib;

public interface IPlayer : IFriendly
{
    ICurrencyModule CurrencyModule { get; }
    IPlayerWeaponModule WeaponsModule { get; }
    IPlayerMovementModule MovementModule { get; }
}
