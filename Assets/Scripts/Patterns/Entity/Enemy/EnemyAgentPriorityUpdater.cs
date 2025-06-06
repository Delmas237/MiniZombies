using System.Collections;
using System.Linq;
using UnityEngine;

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

            var enemies = Spawner<IEnemy>.ObjectsOnScene.Where(
                e => Vector3.Distance(e.Transform.position, e.MoveModule.Target.Transform.position) <= _radius).ToList();

            enemies.Sort((a, b) =>
            {
                float distanceA = Vector3.Distance(a.Transform.position, a.MoveModule.Target.Transform.position);
                float distanceB = Vector3.Distance(b.Transform.position, a.MoveModule.Target.Transform.position);
                return distanceA.CompareTo(distanceB);
            });

            for (int i = enemies.Count - 1; i >= 0; i--)
                enemies[i].MoveModule.Agent.avoidancePriority = i;
        }
    }
}
