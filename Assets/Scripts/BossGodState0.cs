using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGodState0 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<RotateAroundFoward>().enabled = true;
    //    animator.GetComponent<RotateAroundRight>().enabled = true;
    //    animator.GetComponent<RotateAroundUp>().enabled = true;
    }
}
