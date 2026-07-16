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
        private Dictionary<Type, IModule> _interfaceCache = new Dictionary<Type, IModule>();

        public Transform Transform => transform;
        public abstract IEntityHealthModule HealthModule { get; }

        #region Initialization
        private void Awake()
        {
            CollectModules();
            OnAwake();
        }
        private void CollectModules()
        {
            _moduleCache.Clear();
            _interfaceCache.Clear();
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                if (typeof(IModule).IsAssignableFrom(field.FieldType))
                {
                    if (field.GetValue(this) is IModule module)
                    {
                        foreach (var iface in module.GetType().GetInterfaces())
                        {
                            if (typeof(IModule).IsAssignableFrom(iface) && !_interfaceCache.ContainsKey(iface))
                                _interfaceCache[iface] = module;
                        }

                        var moduleType = module.GetType();
                        if (!_moduleCache.TryGetValue(moduleType, out var cache))
                        {
                            cache = new ModuleCache();
                            _moduleCache[moduleType] = cache;
                        }

                        CacheModule(module, cache);
                    }
                }
            }
        }
        private void CacheModule(IModule module, ModuleCache cache)
        {
            cache.Module = module;
            cache.InitialState = module.Enabled;

            cache.Updatable = module as IUpdatable;
            cache.FixedUpdatable = module as IFixedUpdatable;
            cache.LateUpdatable = module as ILateUpdatable;

            cache.OnEnable = module as IOnEnable;
            cache.OnDisable = module as IOnDisable;

            cache.OnCollisionEnter = module as IOnCollisionEnter;
            cache.OnCollisionStay = module as IOnCollisionStay;
            cache.OnCollisionExit = module as IOnCollisionExit;

            cache.OnTriggerEnter = module as IOnTriggerEnter;
            cache.OnTriggerStay = module as IOnTriggerStay;
            cache.OnTriggerExit = module as IOnTriggerExit;

            cache.Disposable = module as IDisposable;
        }
        protected virtual void OnAwake() { }
        #endregion

        #region Lifecycle
        private void Update()
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.Updatable?.Update();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error updating module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.FixedUpdatable?.FixedUpdate();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error fixed updating module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void LateUpdate()
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.LateUpdatable?.LateUpdate();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error late updating module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void OnEnable()
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnEnable?.OnEnable();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnEnable in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void OnDisable()
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnDisable?.OnDisable();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnDisable in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnCollisionEnter?.OnCollisionEnter(collision);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnCollisionEnter in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnCollisionStay?.OnCollisionStay(collision);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnCollisionStay in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnCollisionExit?.OnCollisionExit(collision);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnCollisionExit in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnTriggerEnter?.OnTriggerEnter(other);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnTriggerEnter in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }
        private void OnTriggerStay(Collider other)
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnTriggerStay?.OnTriggerStay(other);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnTriggerStay in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.OnTriggerExit?.OnTriggerExit(other);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error OnTriggerExit in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var cache in _moduleCache.Values)
            {
                try
                {
                    cache.Disposable?.Dispose();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error disposing module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
            _moduleCache.Clear();
            _interfaceCache.Clear();
        }
        #endregion

        #region Public Methods
        public T GetModule<T>() where T : class, IModule
        {
            if (_interfaceCache.TryGetValue(typeof(T), out var module))
                return module as T;
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
            foreach (var cache in _moduleCache.Values)
                cache.Module.Enabled = cache.InitialState;
        }
        #endregion

        public class ModuleCache
        {
            public IModule Module { get; set; }
            public bool InitialState { get; set; }

            public IUpdatable Updatable { get; set; }
            public IFixedUpdatable FixedUpdatable { get; set; }
            public ILateUpdatable LateUpdatable { get; set; }

            public IOnEnable OnEnable { get; set; }
            public IOnDisable OnDisable { get; set; }

            public IOnCollisionEnter OnCollisionEnter { get; set; }
            public IOnCollisionStay OnCollisionStay { get; set; }
            public IOnCollisionExit OnCollisionExit { get; set; }

            public IOnTriggerEnter OnTriggerEnter { get; set; }
            public IOnTriggerStay OnTriggerStay { get; set; }
            public IOnTriggerExit OnTriggerExit { get; set; }

            public IDisposable Disposable { get; set; }
        }
    }
}