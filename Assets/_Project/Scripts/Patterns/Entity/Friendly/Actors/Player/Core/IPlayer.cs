namespace Entity.Friendly.Player
{
    public interface IPlayer : IFriendly
    {
        IPlayerCurrencyModule CurrencyModule { get; }
        IEntityWeaponModule WeaponModule { get; }
        IPlayerMovementModule MovementModule { get; }
    }
}
