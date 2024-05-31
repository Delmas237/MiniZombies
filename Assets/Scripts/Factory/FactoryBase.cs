using UnityEngine;

namespace Factory
{
    public abstract class FactoryBase<T> where T : Component
    {
        public readonly T Prefab;

        public FactoryBase(T prefab)
        {
            Prefab = prefab;
        }

        public virtual T NewInstance() => Object.Instantiate(Prefab);

        protected abstract void ReconstructToDefault(T prefab);
        protected abstract void Construct(T prefab);
    }
}
