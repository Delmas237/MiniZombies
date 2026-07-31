using System;
using UnityEngine;

namespace EntityLib
{
    [Serializable]
    public class EntityRoleModule : IEntityRoleModule
    {
        [SerializeField] private bool _enabled = true;
        [Space(10)]
        [SerializeField] private EntityRole _role;

        public bool Enabled 
        { 
            get => _enabled; 
            set => _enabled = value; 
        }
        public EntityRole Role
        {
            get => Enabled ? _role : EntityRole.None;
            set => _role = value;
        }
    }
}
