using Random=UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Canvas initialCanvas;
    [SerializeField] private Canvas endGameCanvas;
    [SerializeField] private GameObject firstDoor;
    [SerializeField] private GameObject lastDoor;
    [SerializeField] private GameObject bossWings;
    [SerializeField] private GameObject bossLegs;
    [SerializeField] private GameObject bossNeck;
    [SerializeField] private GameObject bossGod;
    [SerializeField] private GameObject ending0Trigger;
    [SerializeField] private GameObject bonfire;
    [SerializeField] private float presentationTime;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float cameraWaitTime;

    [SerializeField] private Vector3 ending1Rotation;
    [SerializeField] private Vector3 ending1Position;
    [SerializeField] private Vector3 ending1Rotation1;
    [SerializeField] private Vector3 ending1Position1;
    [SerializeField] private float end1PosAdjustDuration;

    [SerializeField] private Vector3 ending0Rotation;
    [SerializeField] private Vector3 ending0Position;
    [SerializeField] private float ascendDuration;
    [SerializeField] private CinemachineVirtualCamera ending0Camera;
    [SerializeField] private CinemachineVirtualCamera ending1Camera;
    [SerializeField] private float cameraRotationTurns;
    [SerializeField] private Vector3 targetEnding0Offset;
    [SerializeField] private Vector3 targetEnding1Offset;
    [SerializeField] private float ending0ZoomDuration;
    [SerializeField] private Vector3 ending0RotationAmount;
    [SerializeField] private GameObject skybox;
    [SerializeField] private GameObject starsPs;
    [SerializeField] private GameObject directionalLight;
    private float targetAlpha = 0;
    private Animator cameraAnimator;
    private bool initialScene = true;
    private float alphaModifier = 1;


    public static CameraController instance;
    private bool isEnding0 = false;
    private bool isEnding1 = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        cameraAnimator = this.GetComponent<Animator>();
        player.GetComponentInChildren<Animator>().SetBool("isIdle",true);
        initialCanvas.gameObject.SetActive(true);
        endGameCanvas.gameObject.SetActive(false);
        ending0Trigger.SetActive(false);
        starsPs.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(initialScene){
            if(Input.GetMouseButtonDown(0)){
                initialScene = false;
                player.GetComponentInChildren<Animator>().SetBool("isEating",true);
                player.GetComponentInChildren<Animator>().SetTrigger("eat");
                StartCoroutine(FadeOutAnimation(initialCanvas));
                StartCoroutine(DoAfterTimeCoroutine(fadeOutDuration+cameraWaitTime,() => {
                    StartPlayer();
                }));
            }
        }
        else{
            CheckSetObjectStatus(firstDoor, "firstDoor");
            CheckSetObjectStatus(lastDoor, "lastDoor");
            CheckSetObjectStatus(bossWings, "bossWings");
            CheckSetObjectStatus(bossLegs, "bossLegs");
            CheckSetObjectStatus(bossNeck, "bossNeck");
            CheckSetObjectStatus(bossGod, "bossGod");

            Ending0TriggerCheck();
            Ending1Check();
        }
    }

    private void Ending1Check()
    {
        if(!isEnding1){
            if(bossGod == null){
                StartEnding1();
            }
        }
    }

    private void StartEnding1()
    {
        isEnding1 = true;
        FindObjectOfType<AudioManager>().StopAllExcept(duration: cameraWaitTime);
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Animator>().SetBool("isEating",false);
        player.GetComponentInChildren<Animator>().SetBool("isWalking",false);
        player.GetComponentInChildren<Animator>().SetBool("isFlying",false);
        player.GetComponentInChildren<Animator>().SetTrigger("goIdle");
        player.GetComponentInChildren<Animator>().SetBool("isIdle",false);
        cameraAnimator.SetTrigger("ending1");
        var clipLength = FindObjectOfType<AudioManager>().GetClipLength("endingMusic");directionalLight.GetComponent<RotateAroundUp>().enabled = false;
        StartCoroutine(LerpPlayerPositionRotation(ending1Position,ending1Rotation,end1PosAdjustDuration));
        StartCoroutine(LerpRotationSky(skybox, Vector3.forward * 90,end1PosAdjustDuration));
        StartCoroutine(LerpRotationSky(directionalLight, Vector3.up * -90,end1PosAdjustDuration));
        
        StartCoroutine(DoAfterTimeCoroutine(end1PosAdjustDuration,() => {
            player.GetComponentInChildren<Animator>().enabled = false;
            FindObjectOfType<AudioManager>().PlayDelayed("endingMusic");
            FindObjectOfType<CameraRotator>().ZoomIn(ending1Camera, targetEnding1Offset, clipLength);
            StartCoroutine(LerpPlayerPositionRotation(ending1Position1,ending1Rotation1,clipLength));
            starsPs.SetActive(true);
            FindObjectOfType<CameraRotator>().RotateCamera(cameraRotationTurns/clipLength);
            StartCoroutine(DoAfterTimeCoroutine(clipLength,() => {
                FindObjectOfType<CameraRotator>().StopRotatingCamera();
                var renderers = player.GetComponentsInChildren<Renderer>().ToArray<Renderer>();
                foreach(Renderer renderer in renderers){
                    renderer.enabled = false;
                }
            }));
            StartCoroutine(DoAfterTimeCoroutine(cameraWaitTime*5 + clipLength,() => {
                EndGame();
            }));
        }));
    }

    private void Ending0TriggerCheck()
    {
        if(!isEnding0){
            if(bossWings == null && bossNeck == null && bossLegs == null && lastDoor != null){
                StartEnding0Trigger();
            }
        }
    }

    public void StartEnding0Trigger()
    {
        isEnding0 = true;
        cameraAnimator.SetTrigger("ending0Trigger");   
        StartCoroutine(DoAfterTimeCoroutine(cameraWaitTime,() => {
            ending0Trigger.SetActive(true);
            FindObjectOfType<AudioManager>().PlayDelayed("ending0TriggerSFX");
        }));   
        StartCoroutine(DoAfterTimeCoroutine(3 * cameraWaitTime,() => {
            cameraAnimator.SetTrigger("ending0Trigger"); 
        }));
    }

    public void StartEnding0(){
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Animator>().SetBool("isEating",false);
        player.GetComponentInChildren<Animator>().SetBool("isWalking",false);
        player.GetComponentInChildren<Animator>().SetBool("isFlying",false);
        player.GetComponentInChildren<Animator>().SetTrigger("goIdle");
        player.GetComponentInChildren<Animator>().SetBool("isIdle",false);
        StartCoroutine(LerpPlayerPositionRotation(ending0Position,ending0Rotation,ascendDuration));

        StartCoroutine(DoAfterTimeCoroutine(cameraWaitTime*2,() => {
            cameraAnimator.SetTrigger("ending0");
            player.GetComponentInChildren<Animator>().enabled = false;
            FindObjectOfType<CameraRotator>().ZoomIn(ending0Camera, targetEnding0Offset, ending0ZoomDuration);
            StartCoroutine(DoAfterTimeCoroutine(ending0ZoomDuration/2,() => {
                FindObjectOfType<AudioManager>().PlayDelayed("ghostwhisperSFX");
                FindObjectOfType<AudioManager>().IncreaseVolume("ghostwhisperSFX",1,ending0ZoomDuration/2);
            }));
            StartCoroutine(LerpVolume(bonfire.GetComponent<AudioSource>(),1,ending0ZoomDuration));
            StartCoroutine(DoAfterTimeCoroutine(ending0ZoomDuration + cameraWaitTime*2,() => {
                EndGame();   
            }));    
        }));
    }

    void CheckSetObjectStatus(GameObject gameObject, string name){
        if(gameObject == null){
            cameraAnimator.SetBool(name + "Exist",false);
        }
        else if(gameObject.CompareTag("DeadBoss")){
            cameraAnimator.SetBool(name + "IsDead",true);
        }
    }

    
    IEnumerator FadeOutAnimation(Canvas canvas){
        yield return LerpFunctionCanvas(canvas, targetAlpha, fadeOutDuration);
        canvas.gameObject.SetActive(false);
        Destroy(canvas.gameObject);
    }
    
    void StartPlayer(){
        player.GetComponentInChildren<Animator>().SetBool("isIdle",true);
        cameraAnimator.SetTrigger("startPlayer");
        player.GetComponent<PlayerMovement>().ResetTurboTimer();
        player.GetComponent<PlayerMovement>().enabled = true;
    } 

    public void EndGame(){
        FindObjectOfType<AudioManager>().StopAllExcept();
        player.GetComponent<PlayerMovement>().enabled = false;
        FindObjectOfType<AudioManager>().PlayDelayed("endingClick", 0);
        endGameCanvas.gameObject.SetActive(true);
    }

    public void PresentBoss(string triggerName, Canvas canvas, float delay){
        StartCoroutine(DoAfterTimeCoroutine(delay,() => {
            cameraAnimator.SetTrigger(triggerName);
            StartCoroutine(DoAfterTimeCoroutine(cameraWaitTime,() => {
                canvas.gameObject.SetActive(true);
            }));
            StartCoroutine(DoAfterTimeCoroutine(cameraWaitTime + presentationTime,() => {
                StartCoroutine(FadeOutAnimation(canvas));
            }));
            StartCoroutine(DoAfterTimeCoroutine(presentationTime + fadeOutDuration + cameraWaitTime,() => {
                cameraAnimator.SetTrigger(triggerName);
            }));
        })); 
    }

    IEnumerator LerpFunctionCanvas(Canvas canvas, float endValue, float duration)
    {
        float time = 0;
        float startValue = alphaModifier;
        float startAlpha = canvas.GetComponent<CanvasGroup>().alpha;

        while (time < duration)
        {
            alphaModifier = Mathf.Lerp(startValue, endValue, time / duration);
            canvas.GetComponent<CanvasGroup>().alpha = startAlpha * alphaModifier;
            time += Time.deltaTime;
            yield return null;
        }
        canvas.GetComponent<CanvasGroup>().alpha = startAlpha * targetAlpha;
        // alphaModifier = targetAlpha;
        alphaModifier = 1;
    }

    public static IEnumerator DoAfterTimeCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    IEnumerator LerpPlayerPositionRotation(Vector3 targetPosition, Vector3 targetRotation, float duration)
    {
        float time = 0;
        Vector3 startPosition = player.transform.position;
        Vector3 startRotation = player.transform.rotation.eulerAngles;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            player.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
        player.transform.rotation = Quaternion.Euler(targetRotation);
    }

    IEnumerator LerpRotationSky(GameObject gameObject, Vector3 targetRotation, float duration)
    {
        float time = 0;
        Vector3 startRotation = gameObject.transform.rotation.eulerAngles;

        while (time < duration)
        {
            gameObject.transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.rotation = Quaternion.Euler(targetRotation);
    }
    public void DeactivateAllObjectsExcept(GameObject[] objectsNotToDeactivate)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> objectsToDisable = new List<GameObject>(allObjects);
        foreach(GameObject gameObject in allObjects){
            foreach(GameObject notToDeactivate in objectsNotToDeactivate){
                if(gameObject.transform.root.name == notToDeactivate.transform.root.name){
                    objectsToDisable.Remove(gameObject);
                }
            }
        }
        foreach(GameObject gameObject1 in objectsToDisable){
            gameObject1.SetActive(false);
        }
    }

    IEnumerator LerpVolume(AudioSource source, float endValue, float duration)
    {
        float time = 0;
        float startValue = source.volume;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = endValue;
    }
}
