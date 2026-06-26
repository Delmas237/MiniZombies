using System;
using UnityEngine;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyTargetModule : IEntityTargetModule
    {
        [SerializeField] private bool _enabled = true;

        public bool Enabled { get => _enabled; set => _enabled = value; }
        public bool IsFindingTarget { get; set; } = false;
        public IEntity Target { get; set; }
    }
}
