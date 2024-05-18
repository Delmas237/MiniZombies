using PlayerLib;
using UnityEngine;

public interface IPlayer : IEntity
{
    public ICurrencyController CurrencyController { get; }
    public IPlayerWeaponsController WeaponsController { get; }
    public IPlayerMoveController MoveController { get; }

    public Transform Transform { get; }
}
