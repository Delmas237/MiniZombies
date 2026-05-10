using PlayerLib;

public interface IPlayer : IFriendly
{
    ICurrencyModule CurrencyModule { get; }
    IPlayerWeaponsModule WeaponsModule { get; }
    IPlayerMoveModule MoveModule { get; }
}
