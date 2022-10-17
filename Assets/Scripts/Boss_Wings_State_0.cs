using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Wings_State_0 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<RotateAroundTarget>().enabled = true;
    }
}
