using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeckDeath : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<FollowPlayerPeck>().enabled = false;
    }
}
