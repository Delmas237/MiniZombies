namespace Factory
{
    public interface IFactory<T>
    {
        public T[] Prefabs { get; }

        public T NewInstance();
        public void ReconstructToDefault(T prefab);
        public void Construct(T prefab);
    }
}
