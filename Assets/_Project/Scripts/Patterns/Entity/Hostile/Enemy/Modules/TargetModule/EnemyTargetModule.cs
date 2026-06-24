using System;

namespace Entity.Hostile
{
    [Serializable]
    public class EnemyTargetModule : IEntityTargetModule
    {
        public bool IsFindingTarget { get; set; } = false;

        public IEntity Target { get; set; }
    }
}
