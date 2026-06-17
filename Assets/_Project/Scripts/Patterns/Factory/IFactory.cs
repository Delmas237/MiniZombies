namespace Factory
{
    public interface IFactory<T>
    {
        T[] Prefabs { get; }

        T NewInstance();
        void ReconstructToDefault(T prefab);
        void Construct(T prefab);
    }
}
