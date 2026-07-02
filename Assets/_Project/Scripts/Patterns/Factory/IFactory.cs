namespace Factory
{
    public interface IFactory<T>
    {
        T[] Prefabs { get; }

        T NewInstance();
        void ReconstructToDefault(T instance);
        void Construct(T instance);
    }
}
