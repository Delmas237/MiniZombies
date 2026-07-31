namespace EntityLib
{
    public interface ILateUpdatable : IModuleEvent
    {
        void LateUpdate();
    }
}
