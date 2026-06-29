using System;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyTargetModule : IEntityTargetModule
    {
        [SerializeField] private bool _enabled = true;

        private bool _isFindingTarget;
        private IEntity _target;

        public bool Enabled
        { 
            get => _enabled; 
            set => _enabled = value; 
        }
        public bool IsFindingTarget
        {
            get => _enabled && _isFindingTarget;
            set => _isFindingTarget = value;
        }
        public IEntity Target
        {
            get => _enabled ? _target : null;
            set => _target = value;
        }
    }
}
