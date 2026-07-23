using UnityEngine;

namespace Entity
{
    public interface IEntityRoleModule : IModule
    {
        EntityRole Role { get; set; }
    }
}
