using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPuddle : MonoBehaviour
{
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _cooldown = 0.5f;
    [Space(10)]
    [SerializeField] protected float _lifeTime = 5;
    [Space(10)]
    [SerializeField] private bool _collideWithPlayer = true;
    [SerializeField] private bool _collideWithEnemy;

    private readonly HashSet<IEntity> _collidingEntities = new HashSet<IEntity>();
    private Coroutine _damageCor;

    private void OnEnable()
    {
        StartCoroutine(ManageLifeTime());
    }
    private IEnumerator ManageLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEntity entity))
        {
            if ((entity is IPlayer && _collideWithPlayer) || (entity is IEnemy && _collideWithEnemy))
            {
                _collidingEntities.Add(entity);
                _damageCor ??= StartCoroutine(ManageDamage());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IEntity entity))
        {
            if ((entity is IPlayer && _collideWithPlayer) || (entity is IEnemy && _collideWithEnemy))
            {
                _collidingEntities.Remove(entity);
            }
        }
    }

    private IEnumerator ManageDamage()
    {
        yield return new WaitForSeconds(_cooldown);

        while (_collidingEntities.Count > 0)
        {
            foreach (var entity in _collidingEntities)
                entity.HealthController.Decrease(_damage);

            yield return new WaitForSeconds(_cooldown);
        }
        _damageCor = null;
    }
}
