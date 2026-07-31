using UnityEngine;

namespace EntityLib
{
    public interface IEntityRoleModule : IModule
    {
        EntityRole Role { get; set; }
    }
}
