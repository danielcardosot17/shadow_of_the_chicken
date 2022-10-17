using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLegsDeath : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<FollowPlayerJumping>().enabled = false;
    }
}
