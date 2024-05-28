using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Event : ISubscribable<Action>
{
    public event Action myEvent;

    public void Trigger()
    {
        myEvent?.Invoke();
    }

    public void Subscribe(Action function)
    {
        myEvent += function;
    }

    public void Unsubscribe(Action function)
    {
        myEvent -= function;
    }
}

public class Event<T> : EventBroadcaster<T>
{
    public event Action<T> myEvent;

    public void Trigger(T t)
    {
        myEvent?.Invoke(t);
    }

    public void Subscribe(Action<T> function)
    {
        myEvent += function;
    }

    public void Unsubscribe(Action<T> function)
    {
        myEvent -= function;
    }
}
public class Event<T, U> : ISubscribable<Action<T,U>>
{
    public event Action<T,U> myEvent;

    public void Trigger(T t, U u)
    {
        myEvent?.Invoke(t, u);
    }

    public void Subscribe(Action<T, U> function)
    {
        myEvent += function;
    }

    public void Unsubscribe(Action<T, U> function)
    {
        myEvent -= function;
    }
}
