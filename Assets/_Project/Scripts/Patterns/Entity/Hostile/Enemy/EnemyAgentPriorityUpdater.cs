using System.Collections;
using System.Linq;
using UnityEngine;

namespace Entity.Hostile
{
    public class EnemyAgentPriorityUpdater : MonoBehaviour
    {
        [SerializeField] private float _updateDelay = 1f;
        [SerializeField] private float _radius = 10f;

        private void Start()
        {
            StartCoroutine(UpdatePriority());
        }

        private IEnumerator UpdatePriority()
        {
            while (true)
            {
                yield return new WaitForSeconds(_updateDelay);

                var enemies = EnemySpawner.ObjectsOnScene.Where(e =>
                {
                    IEntity target = e.GetModule<IEntityTargetModule>().Target;
                    float distance = Vector3.Distance(e.Transform.position, target.Transform.position);
                    return distance <= _radius;
                }).ToList();

                enemies.Sort((a, b) =>
                {
                    IEntity target = a.GetModule<IEntityTargetModule>().Target;
                    float distanceA = Vector3.Distance(a.Transform.position, target.Transform.position);
                    float distanceB = Vector3.Distance(b.Transform.position, target.Transform.position);
                    return distanceA.CompareTo(distanceB);
                });

                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    var movementModule = enemies[i].GetModule<IEnemyMovementModule>();
                    movementModule.Agent.avoidancePriority = i;
                }
            }
        }
    }
}
