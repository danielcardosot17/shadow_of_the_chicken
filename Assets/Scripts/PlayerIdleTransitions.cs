using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleTransitions : StateMachineBehaviour
{
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    float timer = 0;
    string[] idle_states = {"idle_0","idle_1","idle_2","idle_3","idle_4","idle_5","idle_6","idle_7","idle_8"}; 

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(timer <= 0){
           StartRandomIdleAnimation(animator);
           timer = Random.Range(minTime,maxTime);
       }
       else{
           timer -= Time.deltaTime;
       }
    }

    private void StartRandomIdleAnimation(Animator animator)
    {
        System.Random rnd = new System.Random();
        int state = rnd.Next(idle_states.Length);
        string trigger = idle_states[state];
        animator.SetTrigger(trigger);
    }
}
