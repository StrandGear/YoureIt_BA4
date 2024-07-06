using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton : MonoBehaviour
{

    private static Dictionary<Type, object> instances;

    public static T GetInstance<T>() where T : Singleton
    {

        // If the instances dictionary hasn't been initialized yet, do so
        if (instances == null)
        {
            instances = new Dictionary<Type, object>();
        }

        // Get the type of the object we're looking for
        Type type = typeof(T);
        T foundInstance = instances.ContainsKey(type) ? (T)instances[type] : null;

        // If the object hasn't been found yet, find it
        if (foundInstance == null)
        {

            T[] foundInstances = FindObjectsOfType<T>();
            if (foundInstances.Length == 0)
            {
                throw new System.Exception("No instances of " + type + " found in scene!");
            }
            else if (foundInstances.Length > 1)
            {
                throw new System.Exception("Multiple instances of " + type + " found in scene! Expected only one.");
            }
            else
            {
                foundInstance = foundInstances[0];
                instances[type] = foundInstance;
            }

        }

        // Return the instance
        return foundInstance;
    }

    protected void Awake()
    {
        // If the instances dictionary hasn't been initialized yet, do so
        if (instances == null)
        {
            instances = new Dictionary<Type, object>();
        }

        // Get the type of the object we're looking for
        Type type = GetType();
        if (instances.ContainsKey(type))
        {
            Destroy(gameObject);
        }
        else
        {
            instances[type] = this;
        }
    }
}