using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLegsState0 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<FollowPlayerJumping>().enabled = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<FollowPlayerJumping>().enabled = false;
    }
}
