namespace Entity
{
    public interface ILateUpdatable : IModuleEvent
    {
        void LateUpdate();
    }
}
