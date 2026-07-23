using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public interface IEntity
    {
        Transform Transform { get; }
        IEntityRoleModule RoleModule { get; }
        IEntityHealthModule HealthModule { get; }

        T GetModule<T>() where T : class, IModule;
        bool HasModule<T>() where T : class, IModule;
        bool TryGetModule<T>(out T module) where T : class, IModule;
        IEnumerable<IModule> GetAllModules();
        void SetAllModulesInitialState();
    }
}
