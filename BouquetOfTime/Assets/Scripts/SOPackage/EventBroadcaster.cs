using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*!Interfaces that allow for easy event assigning to classes that derive from it
 * Subscribe and Unsubscribe methods take a generic action function
 * Implemented by EventSO.cs
 */
public interface EventBroadcaster : ISubscribable<Action>
{

}
public interface EventBroadcaster<T> : ISubscribable<Action<T>>
{
}
public interface EventBroadcaster<T, U> : ISubscribable<Action<T, U>>
{
}

