using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnStateExit : StateMachineBehaviour
{
    [SerializeField] EventSO calledEvent;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        calledEvent.Trigger();
    }
}
