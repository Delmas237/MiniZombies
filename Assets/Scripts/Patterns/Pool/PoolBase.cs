using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ObjectPool
{
    [Serializable]
    public class PoolBase<T> : IPool<T> where T : Component
    {
        [SerializeField] private bool _autoExpand = true;
        public event Action<T> Expanded;

        [SerializeField] protected T[] _prefabs;
        public T[] Prefabs => _prefabs;

        [SerializeField] private int _count = 5;

        private List<T> _pool = new List<T>();
        public IReadOnlyList<T> Elements => _pool;
        [field: Space(10)]
        [field: SerializeField] public Transform Parent { get; set; }

        public void Initialize()
        {
            for (int i = 0; i < _count; i++)
                CreateObject();
        }

        public T GetInstance()
        {
            if (HasFreeElement(out T element))
            {
                if (Parent != null)
                    element.transform.SetParent(Parent);
                return element;
            }

            if (_autoExpand)
            {
                _count++;
                T newElement = CreateObject(true);
                Expanded?.Invoke(newElement);

                return newElement;
            }

            throw new Exception("There is no free elements in pool");
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            int rnd = Random.Range(0, Prefabs.Length);
            T createdObject = Object.Instantiate(Prefabs[rnd]);

            if (Parent != null)
                createdObject.transform.SetParent(Parent);

            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);
            return createdObject;
        }

        public bool HasFreeElement(out T element)
        {
            foreach (T component in _pool)
            {
                if (component.gameObject.activeInHierarchy == false)
                {
                    element = component;
                    component.gameObject.SetActive(true);
                    component.transform.position = Vector3.zero;
                    return true;
                }
            }

            element = null;
            return false;
        }
    }
}
