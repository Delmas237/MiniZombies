using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour
{
    [SerializeField] protected float _cooldown = 3;
    [SerializeField] protected int _maxObjectsOnScene = 50;
    [Space(10)]
    [SerializeField] protected List<Transform> _spawnDots;

    protected static readonly List<T> _objectsOnScene = new List<T>();

    public abstract event Action<T> Spawned;
    public abstract event Action Removed;

    public bool IsSpawn { get; set; } = true;
    
    public float Cooldown => _cooldown;
    public static IReadOnlyList<T> ObjectsOnScene => _objectsOnScene;


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
