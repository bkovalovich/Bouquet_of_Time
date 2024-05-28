using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
/*!A generic class that can schedule events when its value is changed. 
 * COLINHELP - need an explanation on how this class is being used
 */
public class VariableReference<T> : ISerializationCallbackReceiver, ISubscribable<Action<T>>, ISubscribable<Action>
{
    [SerializeField]
    public bool useConstant = false;
    public T constantValue;

    [SerializeField] private IVariableSO<T> variable;
    private IVariableSO<T> prevVariable;

    private event Action<T> ChangeEvent;
    private event Action ChangeEventNullParam;
    /*!implicitly cast C# generic class as custom VariableReference value.
     * This allows for declaring the generic type of a VariableReference variable like any generic variable.
     * COLINHELP - is that right lol
     */
    public static implicit operator T(VariableReference<T> vr) => vr.Value;
    [SerializeField] bool DEBUG;

    public VariableReference(bool useConstant = false)
    {
        this.useConstant = useConstant;
    }

    public T Value
    {
        get
        {
            return GetValue();
        }
        set
        {
            SetValue(value);
        }
    }
    public T GetValue(int depth = 0, int maxDepth = 10)
    {
        return (useConstant || !Variable) ? constantValue : Variable.Value;
    }
    public void SetValue(T value)
    {
        if (useConstant)
        {
            constantValue = value;
            InvokeChangeEvent(value);
        }
        else
        {
            Variable.Value = value;
        }
    }

    public IVariableSO<T> Variable
    {
        get => variable;
        set
        {
            prevVariable?.Unsubscribe(InvokeChangeEvent);
            variable = value;
            prevVariable = variable;
            if (variable)
            {
                variable.Subscribe(InvokeChangeEvent);
                InvokeChangeEvent(variable.Value);
            }
        }
    }

    void OnValidate()
    {
        if (prevVariable != variable)
        {
            Variable = variable;
        }
        if (useConstant)
        {
            InvokeChangeEvent(Value);
        }
    }

    void InvokeChangeEvent(T v)
    {
        if (DEBUG) Debug.Log("invoke change");
        ChangeEvent?.Invoke(v);
        ChangeEventNullParam?.Invoke();
    }

    public void Subscribe(Action<T> function)
    {
        SubscribeWithoutNotify(function);
        Variable = variable;
    }
    public void SubscribeAndCall(Action<T> function)
    {
        SubscribeWithoutNotify(function);
        function(Value);
    }
    public void SubscribeWithoutNotify(Action<T> function)
    {
        ChangeEvent += function;
    }

    public void Unsubscribe(Action<T> function)
    {
        ChangeEvent -= function;
    }
    public void Subscribe(Action function)
    {
        ChangeEventNullParam += function;
        Variable = variable;
    }

    public void Unsubscribe(Action function)
    {
        ChangeEventNullParam -= function;
    }

    //Allows generic classes to receive OnValidate() callbacks
    void ISerializationCallbackReceiver.OnBeforeSerialize() => this.OnValidate();
    void ISerializationCallbackReceiver.OnAfterDeserialize() { }
    public void PrintValue()
    {
        Debug.Log("reference value: "+Value);
    }
}

//Only concrete (non-generic) classes may be serialized.
//Make a new concrete class below for VariableReferences of different types

[System.Serializable]
public class FloatReference : VariableReference<float>
{
    public static implicit operator float(FloatReference f) => f.Value;
    public void Clamp(float min, float max)
    {
        Value = Mathf.Clamp(Value, min, max);
    }
}

[System.Serializable]
public class IntReference : VariableReference<int>
{

}
[System.Serializable]
public class BoolReference : VariableReference<bool>
{
    public BoolReference(bool _useConstant = false) : base(_useConstant)
    {
        
    }
    public void Toggle()
    {
        Value = !Value;
    }
}
[System.Serializable]
public class GameObjectReference : VariableReference<GameObject>
{

}
[System.Serializable]
public class AnimationCurveReference : VariableReference<AnimationCurve>
{

}
