using PlayerLib;

public interface IPlayer : IEntity
{
    public ICurrencyModule CurrencyModule { get; }
    public IPlayerWeaponsModule WeaponsModule { get; }
    public IPlayerMoveModule MoveModule { get; }
}
