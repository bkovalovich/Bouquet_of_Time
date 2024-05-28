using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VariableReferenceSO<T> : IVariableSO<T>, ISubscribable<T>
{
    [SerializeField] protected VariableReference<T> reference;

    public IVariableSO<T> Variable
    {
        get
        {
            return reference.Variable;
        }
        set
        {
            reference.Variable = value;
        }
    }

    public override T GetValue(int depth = 0, int maxDepth = 10)
    {
        return reference.GetValue(depth+1, maxDepth);
    }

    protected override void SetValue(T newValue)
    {
        reference.SetValue(newValue);
    }
}
