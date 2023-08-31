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

        private readonly List<T> pool;

        private readonly bool hasFactory;
        private readonly FactoryBase<T> factory;
        private readonly Transform parent;

        public PoolBase(T prefab, int count, FactoryBase<T> factory)
        {
            Prefab = prefab;

            pool = new List<T>();
            this.factory = factory;
            hasFactory = true;

            for (int i = 0; i < count; i++)
                CreateObject();
        }
        public PoolBase(T prefab, int count, Transform parent)
        {
            Prefab = prefab;

            pool = new List<T>();
            this.parent = parent;
            hasFactory = false;

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

            if (hasFactory)
                createdObject = factory.NewInstance();
            else
                createdObject = Object.Instantiate(Prefab, parent);

            createdObject.gameObject.SetActive(isActiveByDefault);
            pool.Add(createdObject);
            return createdObject;
        }

        public bool HasFreeElement(out T element)
        {
            foreach (T component in pool)
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
