using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeckState0 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<FollowPlayerPeck>().enabled = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<FollowPlayerPeck>().enabled = false;
    }
}
