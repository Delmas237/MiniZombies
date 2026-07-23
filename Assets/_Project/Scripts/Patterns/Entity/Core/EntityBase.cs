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
        private Dictionary<Type, Component> _componentCache = new Dictionary<Type, Component>();

        public Transform Transform => transform;
        public IEntityRoleModule RoleModule => GetModule<IEntityRoleModule>();
        public IEntityHealthModule HealthModule => GetModule<IEntityHealthModule>();

        #region Initialization
        private void Awake()
        {
            CollectModules();
            CollectComponents();
            InitializeModules();

            InvokeOnModules<IOnAwake>(a => a.Awake(), nameof(Awake));
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
            foreach (var iface in module.GetType().GetInterfaces())
            {
                if (typeof(IModuleEvent).IsAssignableFrom(iface))
                    interfaceMap.Add(iface, module);
            }

            ModuleCache moduleCache = new ModuleCache(module, module.Enabled, interfaceMap);
            return moduleCache;
        }

        private void CollectComponents()
        {
            _componentCache.Clear();
            var components = GetComponents<Component>();
            foreach (var comp in components)
            {
                if (comp == null) continue;
                var type = comp.GetType();
                if (!_componentCache.ContainsKey(type))
                    _componentCache[type] = comp;

                foreach (var iface in type.GetInterfaces())
                {
                    if (!_componentCache.ContainsKey(iface))
                        _componentCache[iface] = comp;
                }
            }
        }

        private object ResolveDependency(Type type)
        {
            if (_interfaceCache.TryGetValue(type, out var module))
                return module;
            if (_moduleCache.TryGetValue(type, out var cache))
                return cache.Module;

            if (_componentCache.TryGetValue(type, out var component))
                return component;

            foreach (var kvp in _componentCache)
            {
                if (type.IsAssignableFrom(kvp.Key))
                    return kvp.Value;
            }

            foreach (var cached in _moduleCache.Values)
            {
                if (type.IsAssignableFrom(cached.Module.GetType()))
                    return cached.Module;
                foreach (var iface in cached.Module.GetType().GetInterfaces())
                {
                    if (type.IsAssignableFrom(iface))
                        return cached.Module;
                }
            }

            return null;
        }

        private void AnalyzeDependencies()
        {
            foreach (var cache in _moduleCache.Values)
            {
                var moduleType = cache.Module.GetType();
                var methods = moduleType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    if (method.GetCustomAttribute<ModuleInjectAttribute>() != null)
                    {
                        var parameters = method.GetParameters();

                        if (parameters.Length == 0)
                            break;

                        foreach (var param in parameters)
                        {
                            if (typeof(IModule).IsAssignableFrom(param.ParameterType))
                            {
                                cache.InjectParameters.Add(new InjectParameter
                                {
                                    ParameterType = param.ParameterType,
                                    IsRequired = !param.IsOptional
                                });
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void InitializeModules()
        {
            AnalyzeDependencies();

            var dependencyGraph = BuildDependencyGraph();
            var initialized = InitializeModules(dependencyGraph);

            InitializeRemainingModules(initialized);
        }
        private Dictionary<ModuleCache, List<ModuleCache>> BuildDependencyGraph()
        {
            var graph = new Dictionary<ModuleCache, List<ModuleCache>>();

            foreach (var cache in _moduleCache.Values)
            {
                graph[cache] = GetDependencies(cache);
            }

            return graph;
        }
        private HashSet<ModuleCache> InitializeModules(Dictionary<ModuleCache, List<ModuleCache>> dependencyGraph)
        {
            var initialized = new HashSet<ModuleCache>();
            bool progress;
            int maxIterations = 100;

            do
            {
                progress = false;

                foreach (var cache in _moduleCache.Values)
                {
                    if (!initialized.Contains(cache) &&
                        AreDependenciesInitialized(cache, dependencyGraph, initialized))
                    {
                        TryInitializeModule(cache);
                        initialized.Add(cache);
                        progress = true;
                    }
                }

                maxIterations--;
            }
            while (progress && maxIterations > 0);

            return initialized;
        }
        private bool AreDependenciesInitialized(ModuleCache cache, Dictionary<ModuleCache, List<ModuleCache>> dependencyGraph, HashSet<ModuleCache> initialized)
        {
            foreach (var dependency in dependencyGraph[cache])
            {
                if (!initialized.Contains(dependency))
                    return false;
            }
            return true;
        }
        private void TryInitializeModule(ModuleCache cache)
        {
            try
            {
                InvokeInjectMethod(cache);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error initializing module {cache.Module.GetType().Name}: {e.Message}");
            }
        }
        private void InitializeRemainingModules(HashSet<ModuleCache> initialized)
        {
            if (initialized.Count == _moduleCache.Count)
                return;

            var remaining = _moduleCache.Values
                .Where(c => !initialized.Contains(c))
                .ToList();

            Debug.LogError($"Cyclic dependency detected! Remaining modules: " +
                $"{string.Join(", ", remaining.Select(c => c.Module.GetType().Name))}");

            foreach (var cache in remaining)
            {
                try
                {
                    InvokeInjectMethod(cache);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to initialize {cache.Module.GetType().Name}: {e.Message}");
                }
            }
        }

        private List<ModuleCache> GetDependencies(ModuleCache cache)
        {
            var dependencies = new List<ModuleCache>();

            foreach (var param in cache.InjectParameters)
            {
                var dependency = FindModuleByType(param.ParameterType);

                if (dependency != null && dependency != cache)
                {
                    if (!dependencies.Contains(dependency))
                        dependencies.Add(dependency);
                }
                else if (param.IsRequired && ResolveDependency(param.ParameterType) == null)
                {
                    Debug.LogError(
                        $"Required dependency {param.ParameterType.Name} not found for module {cache.Module.GetType().Name}");
                }
            }

            return dependencies;
        }

        private ModuleCache FindModuleByType(Type type)
        {
            foreach (var cache in _moduleCache.Values)
            {
                if (cache.Module.GetType() == type)
                    return cache;

                foreach (var iface in cache.Module.GetType().GetInterfaces())
                {
                    if (iface == type)
                        return cache;
                }
            }

            foreach (var cache in _moduleCache.Values)
            {
                if (type.IsAssignableFrom(cache.Module.GetType()))
                    return cache;
            }

            return null;
        }

        private void InvokeInjectMethod(ModuleCache cache)
        {
            var moduleType = cache.Module.GetType();
            var methods = moduleType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<ModuleInjectAttribute>() != null)
                {
                    var parameters = method.GetParameters();
                    var args = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramType = parameters[i].ParameterType;
                        args[i] = ResolveDependency(paramType);
                        if (args[i] == null && !parameters[i].IsOptional)
                            throw new InvalidOperationException($"Cannot resolve {paramType.Name} for {moduleType.Name}");
                    }

                    try
                    {
                        method.Invoke(cache.Module, args);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error initializing module {moduleType.Name}: {e.Message}\n" +
                                       $"Inner: {e.InnerException?.Message}\n" +
                                       $"StackTrace: {e.InnerException?.StackTrace}");
                        throw;
                    }
                    break;
                }
            }
        }
        #endregion

        #region Lifecycle
        private void Start() => InvokeOnModules<IOnStart>(s => s.Start(), nameof(Start));

        private void Update() => InvokeOnModules<IUpdatable>(u => u.Update(), nameof(Update));
        private void FixedUpdate() => InvokeOnModules<IFixedUpdatable>(f => f.FixedUpdate(), nameof(FixedUpdate));
        private void LateUpdate() => InvokeOnModules<ILateUpdatable>(l => l.LateUpdate(), nameof(LateUpdate));

        private void OnEnable() => InvokeOnModules<IOnEnable>(e => e.OnEnable(), nameof(OnEnable));
        private void OnDisable() => InvokeOnModules<IOnDisable>(d => d.OnDisable(), nameof(OnDisable));

        private void OnCollisionEnter(Collision collision) =>
            InvokeOnModules<IOnCollisionEnter>(c => c.OnCollisionEnter(collision), nameof(OnCollisionEnter));
        private void OnCollisionStay(Collision collision) =>
            InvokeOnModules<IOnCollisionStay>(c => c.OnCollisionStay(collision), nameof(OnCollisionStay));
        private void OnCollisionExit(Collision collision) =>
            InvokeOnModules<IOnCollisionExit>(c => c.OnCollisionExit(collision), nameof(OnCollisionExit));

        private void OnTriggerEnter(Collider other) =>
            InvokeOnModules<IOnTriggerEnter>(t => t.OnTriggerEnter(other), nameof(OnTriggerEnter));
        private void OnTriggerStay(Collider other) =>
            InvokeOnModules<IOnTriggerStay>(t => t.OnTriggerStay(other), nameof(OnTriggerStay));
        private void OnTriggerExit(Collider other) =>
            InvokeOnModules<IOnTriggerExit>(t => t.OnTriggerExit(other), nameof(OnTriggerExit));

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
        /// <summary>
        /// Gets a required module by its interface.
        /// Throws an exception if the module is not found.
        /// Use this method when the module is mandatory for the entity.
        /// </summary>
        public T GetModule<T>() where T : class, IModule
        {
            Type type = typeof(T);

            if (!type.IsInterface)
                throw new InvalidOperationException(
                    $"{nameof(GetModule)}<{type.Name}> supports only module interfaces");

            if (_interfaceCache.TryGetValue(type, out var module))
                return module as T;

            throw new InvalidOperationException(
                $"Required module {type.Name} not found on entity {name}");
        }

        /// <summary>
        /// Checks if the entity contains a module with the specified interface.
        /// Use this method for checking optional module availability.
        /// </summary>
        public bool HasModule<T>() where T : class, IModule
        {
            Type type = typeof(T);

            if (!type.IsInterface)
                throw new InvalidOperationException(
                    $"{nameof(HasModule)}<{type.Name}> supports only module interfaces");

            return _interfaceCache.ContainsKey(type);
        }

        /// <summary>
        /// Tries to get an optional module by its interface.
        /// Returns true if the module exists, otherwise returns false.
        /// Use this method when the module is not mandatory for the entity.
        /// </summary>
        public bool TryGetModule<T>(out T module) where T : class, IModule
        {
            Type type = typeof(T);

            if (!type.IsInterface)
                throw new InvalidOperationException(
                    $"{nameof(T)}<{type.Name}> supports only module interfaces");

            if (_interfaceCache.TryGetValue(type, out var value))
            {
                module = value as T;
                return true;
            }

            module = null;
            return false;
        }

        /// <summary>
        /// Returns all modules attached to the entity.
        /// Use this method only when iterating through the entire module collection is required.
        /// </summary>
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
            public List<InjectParameter> InjectParameters { get; set; } = new List<InjectParameter>();

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

        public class InjectParameter
        {
            public Type ParameterType { get; set; }
            public bool IsRequired { get; set; }
        }
    }
}
