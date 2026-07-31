using UnityEngine;

namespace EntityLib
{
    public abstract class ModuleWrapper<T> : MonoBehaviour, IModuleWrapper where T : IModule
    {
        [SerializeField] private T _module;

        public IModule Module => _module;
    }
}
