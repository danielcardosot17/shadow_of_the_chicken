using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWind : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject windLoop;
    [SerializeField] private GameObject windNoLoop;
    
    [Range(2f,4f)]
    [SerializeField] private float minTime;
    
    [Range(4f,8f)]
    [SerializeField] private float maxTime;
    
    [Range(2,6)]
    [SerializeField] private int windCount;
    private bool canEmit = true;
    private float windEmissionTimer;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        if(canEmit){
            canEmit = false;
            EmitWind();
            windEmissionTimer = UnityEngine.Random.Range(minTime,maxTime);
            StartCoroutine(DoAfterTimeCoroutine(windEmissionTimer,() => {
                canEmit = true; 
            }));
        }  
    }
    void EmitWind(){
        var randomCount = UnityEngine.Random.Range(1, windCount);
        for(int i = 0; i < randomCount; i++){
            Instantiate(windLoop,transform.position, transform.rotation); 
            Instantiate(windNoLoop,transform.position, transform.rotation); 
        }
    }
    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
