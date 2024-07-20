using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnStateEnter : StateMachineBehaviour
{
    [SerializeField] EventSO calledEvent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        calledEvent.Trigger();

    }
}
