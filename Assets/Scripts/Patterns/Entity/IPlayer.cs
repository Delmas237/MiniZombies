using PlayerLib;

public interface IPlayer : IFriendly
{
    public ICurrencyModule CurrencyModule { get; }
    public IPlayerWeaponsModule WeaponsModule { get; }
    public IPlayerMoveModule MoveModule { get; }
}
