using PlayerLib;

public interface IPlayer : IEntity
{
    public ICurrencyController CurrencyController { get; }
    public IPlayerWeaponsController WeaponsController { get; }
    public IPlayerMoveController MoveController { get; }
}
