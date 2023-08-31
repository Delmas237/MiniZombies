using System;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentSearcher<T> where T : Component
{
    public static T Closest(Vector3 pos, List<T> components) => Search(pos, components, Desired.Closest);
    public static T Closest(Vector3 pos, float radius) => Search(pos, radius, Desired.Closest);

    public static T Furthest(Vector3 pos, List<T> components) => Search(pos, components, Desired.Furthest);
    public static T Furthest(Vector3 pos, float radius) => Search(pos, radius, Desired.Furthest);

    private static T Search(Vector3 pos, List<T> components, Desired type)
    {
        if (components == null)
            throw new NullReferenceException("Components is null, cant search");

        float maxDistanceToComponent;
        if (type == Desired.Closest)
            maxDistanceToComponent = Mathf.Infinity;
        else
            maxDistanceToComponent = Mathf.NegativeInfinity;

        T component = null;
        for (int i = 0; i < components.Count; i++)
        {
            float distanceToComponent = Vector3.Distance(components[i].transform.position, pos);

            if (type == Desired.Closest)
            {
                if (distanceToComponent < maxDistanceToComponent)
                {
                    maxDistanceToComponent = distanceToComponent;
                    component = components[i];
                }
            }
            else
            {
                if (distanceToComponent > maxDistanceToComponent)
                {
                    maxDistanceToComponent = distanceToComponent;
                    component = components[i];
                }
            }
        }

        return component;
    }
    private static T Search(Vector3 pos, float radius, Desired type)
    {
        InRadius(pos, radius, out List<T> components);
        return Search(pos, components, type);
    }

    public static int InRadius(Vector3 pos, float radius, out List<T> results, bool findOnlyEnabled = true)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        results = new List<T>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out T component))
            {
                if (findOnlyEnabled)
                {
                    Behaviour behaviour = component as Behaviour;

                    if (behaviour)
                    {
                        if (behaviour.enabled)
                            results.Add(component);
                    }
                    else
                    {
                        findOnlyEnabled = false;
                        results.Add(component);
                    }
                }
                else
                {
                    results.Add(component);
                }
            }
        }

        return results.Count;
    }

    private enum Desired { Closest, Furthest }
}
