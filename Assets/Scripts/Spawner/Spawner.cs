using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour
{
    [field: SerializeField] public bool IsSpawn { get; set; } = true;
    [field: SerializeField] public float Cooldown { get; set; } = 3;

    [SerializeField] protected int _maxObjectsOnScene = 50;

    public abstract event Action<T> Spawned;
    public abstract event Action Removed;
    
    protected static readonly List<T> _objectsOnScene = new List<T>();
    public static IReadOnlyList<T> ObjectsOnScene => _objectsOnScene;

    [SerializeField] protected List<Transform> _spawnDots;

    protected virtual void Start()
    {
        StartCoroutine(SpawnController());
    }

    protected IEnumerator SpawnController()
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown);

            if (IsSpawn && _objectsOnScene.Count < _maxObjectsOnScene)
                Spawn();
        }
    }
    protected abstract void Spawn();

    protected virtual void OnDestroy()
    {
        _objectsOnScene.Clear();
    }
}
