using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "EventSO", menuName = "EventsSO/Void", order = 1)]
/*!A script that streamlines the event system for TimeWorks.
 * Creating an instance of this SO allows any object to subscribe to the same event. 
 * Has two classes, one that takes a generic and one that doesn't 
 */
public class EventSO : ScriptableObject, EventBroadcaster
{
    [TextArea(3,5)]
    [SerializeField] string description;
    [SerializeField] bool debug;

    private Event Event = new Event();

    public void Subscribe(Action function)
    {
        Event.Subscribe(function);
    }

    public void Unsubscribe(Action function)
    {
        Event.Unsubscribe(function);
    }
    /*!The method that triggers the event and notifies other objects subscribed to it*/
    public void Trigger()
    {
        if (debug) Debug.Log(name + " event fired");
        Event.Trigger();
    }
}


public class EventSO<T> : ScriptableObject, EventBroadcaster<T>
{
    public Event<T> Event = new Event<T>();
    [TextArea(3, 5)]
    [SerializeField] string description;

    public void Trigger(T arg)
    {
        Event.Trigger(arg);
    }

    public void Subscribe(Action<T> function)
    {
        Event.Subscribe(function);
    }

    public void Unsubscribe(Action<T> function)
    {
        Event.Unsubscribe(function);
    }
}