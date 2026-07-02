using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Entity
{
    public abstract class EntityBase : MonoBehaviour, IEntity
    {
        private Dictionary<Type, ModuleCache> _moduleCache = new Dictionary<Type, ModuleCache>();

        public Transform Transform => transform;
        public abstract IEntityHealthModule HealthModule { get; }
        
        private void Awake()
        {
            CollectModules();
            OnAwake();
        }
        private void CollectModules()
        {
            _moduleCache.Clear();
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                if (typeof(IModule).IsAssignableFrom(field.FieldType))
                {
                    if (field.GetValue(this) is IModule module)
                    {
                        var moduleType = module.GetType();
                        _moduleCache.TryGetValue(moduleType, out ModuleCache cache);

                        if (cache == null)
                        {
                            cache = new ModuleCache();
                            _moduleCache[moduleType] = cache;
                        }

                        cache.Module = module;
                        cache.InitialState = module.Enabled;
                    }
                }
            }
        }
        protected virtual void OnAwake() { }


        protected virtual void OnDestroy()
        {
            DisposeAllModules();
        }
        private void DisposeAllModules()
        {
            foreach (var kvp in _moduleCache)
            {
                var module = kvp.Value as IDisposable;
                try
                {
                    module?.Dispose();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error disposing module: {e.Message}");
                }
            }
            _moduleCache.Clear();
        }


        public T GetModule<T>() where T : class, IModule
        {
            var moduleType = typeof(T);
            if (_moduleCache.TryGetValue(moduleType, out var cache))
            {
                return cache.Module as T;
            }

            foreach (var cachedModule in _moduleCache.Values)
            {
                if (cachedModule is T typedModule)
                    return typedModule;
            }

            return null;
        }

        public bool HasModule<T>() where T : class, IModule
        {
            return GetModule<T>() != null;
        }

        public IEnumerable<IModule> GetAllModules()
        {
            return _moduleCache.Values.Select(c => c.Module);
        }

        public void SetAllModulesInitialState()
        {
            foreach (var caches in _moduleCache.Values)
                caches.Module.Enabled = caches.InitialState;
        }

        public class ModuleCache
        {
            public IModule Module { get; set; }
            public bool InitialState { get; set; }
        }
    }
}
