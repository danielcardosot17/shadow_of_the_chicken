using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Boss_God : MonoBehaviour
{
    [SerializeField] private Color killColor;
    [SerializeField] private float shrinkDuration;
    [SerializeField] private float targetScale;
    [SerializeField] private float cameraWaitTime;
    [SerializeField] private GameObject deathExplosionFX;
    [SerializeField] private float chickenCockDelay;
    private Transform[] killPlataforms;
    private int bluePlataforms, redPlataforms = 0;
    private int state = 0;
    private Animator animator;
    private bool isDead = false;
    private float scaleModifier = 1;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        animator = this.GetComponent<Animator>();
        animator.SetBool("isDead",isDead);
        // animator.SetInteger("Boss_God_State",state);
        killPlataforms = this.GetComponentsInChildren<Transform>().Where(x => x.CompareTag("KillBossPlataform")).ToArray();
        bluePlataforms = killPlataforms.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDead){
            if(redPlataforms == bluePlataforms){
                Die();
            }
            var countRed = 0;
            foreach(Transform plataform in killPlataforms){
                if(plataform.GetComponent<Renderer>().material.color == killColor){
                    countRed++;
                }
            }
            redPlataforms = countRed;
            if(state != redPlataforms){
                state = redPlataforms;
                // animator.SetInteger("Boss_God_State",state);
            }
        }
    }
    public void Die(){
        isDead = true;
        animator.SetBool("isDead",isDead);
        // unparent player
        var player = this.GetComponentsInChildren<Transform>().Where(x => x.CompareTag("Player")).FirstOrDefault();
        if(player != null){
            player.parent = null;
        }
        this.tag = "DeadBoss";
        FindObjectOfType<AudioManager>().PlayDelayed("chickenCock", chickenCockDelay);
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation(){
        yield return LerpFunction(targetScale, shrinkDuration);
        Instantiate(deathExplosionFX,transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().PlayDelayed("bossDeathPuff");
        var animationTime = deathExplosionFX.GetComponent<ParticleSystem>().main.duration;
        var renderers = this.gameObject.GetComponentsInChildren<Renderer>().ToArray<Renderer>();
        foreach(Renderer renderer in renderers){
            renderer.enabled = false;
        }
        Destroy(this.gameObject,animationTime+cameraWaitTime);
    }

    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = scaleModifier;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endValue, time / duration);
            transform.localScale = startScale * scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = startScale * targetScale;
        scaleModifier = targetScale;
    }
    
}
