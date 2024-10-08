using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bouquet
{
    /// <summary>\
    /// A monobehavior that reads and broadcasts animation events from the animator to wherever else
    /// This component must go on the game object that hosts an animator
    /// </summary>
    public class AnimationEventBroadcaster : MonoBehaviour
    {
        public UnityEvent DodgeFinishedEvent;
        public UnityEvent AttackFinishedEvent;
        public UnityEvent WindUpFinishedEvent;
        public UnityEvent<float> StartHitboxEvent;
        public UnityEvent AttackCompleteEvent;

        public void DodgeFinished()
        {
            DodgeFinishedEvent?.Invoke();
        }

        public void AttackFinished()
        {
            AttackFinishedEvent?.Invoke(); 
        }

        public void WindUpFinished()
        {
            WindUpFinishedEvent?.Invoke();
        }

        public void StartHitbox(float time)
        {
            StartHitboxEvent?.Invoke(time);
        }

        public void AttackComplete()
        {
            AttackCompleteEvent?.Invoke();
        }
    }
}
