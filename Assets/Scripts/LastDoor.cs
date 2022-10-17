using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
    [SerializeField] private Color killColor;
    [SerializeField] private float shrinkDuration;
    [SerializeField] private float targetScale;
    [SerializeField] private float cameraWaitTime;
    [SerializeField] private GameObject ending1Trigger;
    [SerializeField] private GameObject ending0Trigger;
    private Transform[] killPlataforms;
    private int bluePlataforms, redPlataforms = 0;
    private Animator animator;
    private bool isDead = false;
    private float scaleModifier = 1;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        animator = this.GetComponent<Animator>();
        animator.SetBool("isDead",isDead);
        killPlataforms = this.GetComponentsInChildren<Transform>().Where(x => x.CompareTag("KillBossPlataform")).ToArray();
        bluePlataforms = killPlataforms.Length;
        ending1Trigger.SetActive(false);
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
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation(){
        yield return LerpFunction(targetScale, shrinkDuration);
        // Instantiate(deathExplosionFX,transform.position, Quaternion.Euler(-90,0,0));
        ending0Trigger.SetActive(false);
        FindObjectOfType<AudioManager>().PlayDelayed("lastDoorSFX");
        ending1Trigger.SetActive(true);
        // var animationTime = deathExplosionFX.GetComponent<ParticleSystem>().main.duration;
        var renderers = this.gameObject.GetComponentsInChildren<Renderer>().ToArray<Renderer>();
        foreach(Renderer renderer in renderers){
            renderer.enabled = false;
        }
        Destroy(this.gameObject,cameraWaitTime);
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
    void DestroyBossAfterTime(GameObject boss, float animationTime){
        var renderers = boss.GetComponentsInChildren<Renderer>().ToArray<Renderer>();
        foreach(Renderer renderer in renderers){
            renderer.enabled = false;
        }
        Destroy(boss,animationTime);
    }

    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
