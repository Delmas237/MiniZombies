using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventBusLib
{
    public static class EventBus
    {
        private static Dictionary<Type, List<object>> _eventCallbacks = new Dictionary<Type, List<object>>();

        public static void Subscribe<T>(Action<T> callback)
        {
            Type type = typeof(T);
            if (_eventCallbacks.ContainsKey(type))
            {
                _eventCallbacks[type].Add(callback);
            }
            else
            {
                _eventCallbacks.Add(type, new List<object>() { callback });
            }
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            Type type = typeof(T);
            if (_eventCallbacks.ContainsKey(type))
            {
                _eventCallbacks[type].Remove(callback);
            }
            else
            {
                Debug.LogError($"Event {type} does not exist");
            }
        }

        public static void Invoke<T>(T signal)
        {
            Type type = typeof(T);
            if (_eventCallbacks.ContainsKey(type))
            {
                List<object> callbacks = _eventCallbacks[type].ToList();
                foreach (var callback in callbacks)
                {
                    var callbackAction = callback as Action<T>;
                    callbackAction?.Invoke(signal);
                }
            }
        }
    }
}
