using UnityEngine;

namespace Factory
{
    public abstract class FactoryBase<T> where T : Component
    {
        public readonly T Prefab;
        public readonly Transform Parent;

        public FactoryBase(T prefab, Transform parent)
        {
            Prefab = prefab;
            Parent = parent;
        }

        public virtual T NewInstance() => Object.Instantiate(Prefab, Parent);

        public abstract void ReconstructToDefault(T prefab);
        public abstract void Construct(T prefab);
    }
}
