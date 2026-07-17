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
                        ModuleCache cache = GetCache(module);
                        _moduleCache[moduleType] = cache;
                    }
                }
            }
        }
        private ModuleCache GetCache(IModule module)
        {
            Dictionary<Type, object> interfaceMap = new Dictionary<Type, object>();
             
            FillMap<IUpdatable>();
            FillMap<IFixedUpdatable>();
            FillMap<ILateUpdatable>();

            FillMap<IOnEnable>();
            FillMap<IOnDisable>();

            FillMap<IOnCollisionEnter>();
            FillMap<IOnCollisionStay>();
            FillMap<IOnCollisionExit>();

            FillMap<IOnTriggerEnter>();
            FillMap<IOnTriggerStay>();
            FillMap<IOnTriggerExit>();

            FillMap<IDisposable>();

            ModuleCache moduleCache = new ModuleCache(module, module.Enabled, interfaceMap);
            return moduleCache;

            void FillMap<T>() where T : class => 
                interfaceMap[typeof (T)] = module as T;
        }
        protected virtual void OnAwake() { }
        #endregion

        #region Lifecycle
        private void Update() => InvokeOnModules<IUpdatable>(u => u.Update(), nameof(Update));
        private void FixedUpdate() => InvokeOnModules<IFixedUpdatable>(u => u.FixedUpdate(), nameof(FixedUpdate));
        private void LateUpdate() => InvokeOnModules<ILateUpdatable>(u => u.LateUpdate(), nameof(LateUpdate));

        private void OnEnable() => InvokeOnModules<IOnEnable>(u => u.OnEnable(), nameof(OnEnable));
        private void OnDisable() => InvokeOnModules<IOnDisable>(u => u.OnDisable(), nameof(OnDisable));

        private void OnCollisionEnter(Collision collision) =>
            InvokeOnModules<IOnCollisionEnter>(u => u.OnCollisionEnter(collision), nameof(OnCollisionEnter));
        private void OnCollisionStay(Collision collision) =>
            InvokeOnModules<IOnCollisionStay>(u => u.OnCollisionStay(collision), nameof(OnCollisionStay));
        private void OnCollisionExit(Collision collision) =>
            InvokeOnModules<IOnCollisionExit>(u => u.OnCollisionExit(collision), nameof(OnCollisionExit));

        private void OnTriggerEnter(Collider other) =>
            InvokeOnModules<IOnTriggerEnter>(u => u.OnTriggerEnter(other), nameof(OnTriggerEnter));
        private void OnTriggerStay(Collider other) =>
            InvokeOnModules<IOnTriggerStay>(u => u.OnTriggerStay(other), nameof(OnTriggerStay));
        private void OnTriggerExit(Collider other) =>
            InvokeOnModules<IOnTriggerExit>(u => u.OnTriggerExit(other), nameof(OnTriggerExit));

        protected virtual void OnDestroy() => Dispose();
        private void Dispose()
        {
            InvokeOnModules<IDisposable>(d => d.Dispose(), nameof(Dispose), true);
            _moduleCache.Clear();
            _interfaceCache.Clear();
        }

        private void InvokeOnModules<TInterface>(Action<TInterface> action, string methodName, bool ignoreState = false) where TInterface : class
        {
            foreach (var cache in _moduleCache.Values)
            {
                if (!cache.Module.Enabled && !ignoreState)
                    continue;

                var handler = cache.GetInterface<TInterface>();
                if (handler == null)
                    continue;

                try
                {
                    action(handler);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error {methodName} in module {cache.Module.GetType().Name}: {e.Message}");
                }
            }
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
            private Dictionary<Type, object> _interfaceMap = new Dictionary<Type, object>();

            public IModule Module { get; set; }
            public bool InitialState { get; set; }

            public ModuleCache(IModule module, bool initialState, Dictionary<Type, object> interfaceMap)
            {
                Module = module;
                InitialState = initialState;
                _interfaceMap = interfaceMap;
            }

            public T GetInterface<T>() where T : class
            {
                _interfaceMap.TryGetValue(typeof(T), out var obj);
                return obj as T;
            }
        }
    }
}