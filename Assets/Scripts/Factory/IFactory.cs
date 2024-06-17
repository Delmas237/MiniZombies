namespace Factory
{
    public interface IFactory<T>
    {
        public T Prefab { get; }

        public T NewInstance();
        public void ReconstructToDefault(T prefab);
        public void Construct(T prefab);
    }
}
