using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Variable/Bool")]
/*!VariableSO for bool variables, has a method to toggle the value as true/false
 */
public class BoolVariableSO : VariableSO<bool>
{
    public void Toggle() {
        Value = !Value;
    }
    public void SetRaw(bool val) {
        Value = val;
    }
    public bool GetRawValue() {
        return value;
    }
}