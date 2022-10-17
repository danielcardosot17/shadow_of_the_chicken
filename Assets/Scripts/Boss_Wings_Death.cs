using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Wings_Death : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<RotateAroundTarget>().enabled = false;
       animator.GetComponent<RotateAroundFoward>().enabled = false;
       animator.GetComponent<OscillateUpDown>().enabled = false;
       
    }
}
