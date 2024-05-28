using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Save a variable as a ScriptableObject asset that can be referenced from multiple scripts.
/// Recieve a callback when this value changes by using Subscribe() and Unsubscribe()
/// Param is "InputAction.CallbackContext context"
/// </summary>
/// <typeparam name="T"></typeparam>
public class VariableSO<T> : IVariableSO<T>, ISubscribable<Action<T>>
{
    [SerializeField] protected T value;
    [SerializeField] bool DEBUG = false;


    protected override void SetValue(T t)
    {
        value = t;
        if (DEBUG) {
            Debug.Log(name + " changed to " + value);
        }

    }
    protected override void InvokeChangeEvent(T v)
    {
        base.InvokeChangeEvent(v);

    }

    public override T GetValue(int depth, int maxDepth)
    {
        return value;
    }
}
