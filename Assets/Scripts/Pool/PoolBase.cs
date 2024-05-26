using Factory;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ObjectPool
{
    public class PoolBase<T> : IPool<T> where T : Component
    {
        public T Prefab { get; }
        public bool AutoExpand { get; set; }

        private readonly List<T> _pool;

        private readonly bool _hasFactory;
        private readonly FactoryBase<T> _factory;
        private readonly Transform _parent;

        public PoolBase(T prefab, int count, FactoryBase<T> factory, Transform parent)
        {
            Prefab = prefab;

            _pool = new List<T>();
            _parent = parent;
            _factory = factory;
            _hasFactory = true;

            for (int i = 0; i < count; i++)
                CreateObject();
        }
        public PoolBase(T prefab, int count, Transform parent)
        {
            Prefab = prefab;

            _pool = new List<T>();
            _parent = parent;
            _hasFactory = false;

            for (int i = 0; i < count; i++)
                CreateObject();
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out T element))
                return element;

            if (AutoExpand)
                return CreateObject(true);

            throw new Exception("There is no free elements in pool");
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            T createdObject;

            if (_hasFactory)
                createdObject = _factory.NewInstance();
            else
                createdObject = Object.Instantiate(Prefab);

            createdObject.transform.SetParent(_parent);
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
                    return true;
                }
            }

            element = null;
            return false;
        }
    }
}
