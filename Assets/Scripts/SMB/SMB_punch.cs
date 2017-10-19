using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_punch : StateMachineBehaviour {

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("punch", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventManager.DispatchEvent(Events.ON_ATTACK_EXIT);
    }

}
