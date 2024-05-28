using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*!Interface that is implemented by all event based scriptable objects in the project. Outlines subscribe methods that take a generic function. 
 */
public interface ISubscribable<T>
{
    public void Subscribe(T function)
    {
    }

    public void Unsubscribe(T function)
    {
    }
}