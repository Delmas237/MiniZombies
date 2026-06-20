namespace Entity
{
    public interface IEntityMovementModule
    {
        float DefaultSpeed { get; }
        void Move();
    }
}
