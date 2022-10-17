using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController)) ]
public class PlayerMovement : MonoBehaviour
{
    private Animator playerAnimator;
    private bool isGrounded = true;
    private float groundedTimer;
    private CharacterController characterController;
    [Header("PARAMETROS DO PULO")]
    [SerializeField] private float groundedBufferTime;
    [SerializeField] private float planeSpeed;
    [SerializeField] private float turboSpeed;
    [SerializeField] private float turboBufferTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;

    [Range(0.1f,5f)]
    [SerializeField] private float amplitude;
    
    [Range(0.1f,5f)]
    [SerializeField] private float frequency;
    [SerializeField] private Color killColor;

    [Range(0.1f,0.9f)]
    [SerializeField] private float glideFactor;
    private Camera playerCamera;
    private float verticalSpeed = 0.0f;
    private float turboTimer = 0.0f;
    private Vector3 originalScale;
    private bool isShaking = false;
    private bool isGliding = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!Application.isEditor){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        originalScale = transform.localScale;
        playerAnimator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        if(playerCamera==null){
            playerCamera = Camera.main;
        }
    }
    void FixedUpdate()
    {
        if(transform.root.CompareTag("Boss")){
            if(!isShaking){
                FindObjectOfType<CinemachineShake>().StartShake(amplitude, frequency);
                isShaking = true;
            } 
        }
        else{
            if(isShaking){
                FindObjectOfType<CinemachineShake>().StopShake();
                isShaking = false;
            }
        }
        transform.localScale = originalScale;
        if(characterController.isGrounded){
            verticalSpeed=0;
            groundedTimer=0;
            isGrounded=true;
        }
        else{
            groundedTimer += Time.deltaTime;
            if(groundedTimer>=groundedBufferTime){
                isGrounded=false;
            }
        }

        var direction =
                Vector3.ProjectOnPlane(playerCamera.transform.forward,Vector3.up).normalized * Input.GetAxis("Vertical") + playerCamera.transform.right * Input.GetAxis("Horizontal");
        var planeMove = direction * planeSpeed * Time.deltaTime;

        if(Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")) >= 1){
            if(turboTimer > 0){
                turboTimer -= Time.deltaTime;
            }
            else{
                planeMove = direction * turboSpeed * Time.deltaTime;
            }
        }
        else{
            turboTimer = turboBufferTime;
        }

        if(isGrounded){
            playerAnimator.SetBool("isWalking",direction.magnitude!=0);
            playerAnimator.SetBool("isFlying",false);
            playerAnimator.SetBool("isEating",false);
            isGliding = false;
            if(Input.GetMouseButtonDown(0)){
                playerAnimator.SetBool("isWalking",false);
                playerAnimator.SetBool("isFlying",false);
                verticalSpeed=0;
                planeMove = Vector3.zero;
                playerAnimator.SetBool("isEating",true);
                playerAnimator.SetTrigger("eat");
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                verticalSpeed=Mathf.Sqrt(2*gravity*jumpHeight);
                isGrounded=false;
            }
        }
        else{
            if(Input.GetKey(KeyCode.Space)){
                isGliding = true;
            }
            else{
                isGliding = false;
            }
            playerAnimator.SetBool("isEating",false);
            playerAnimator.SetBool("isWalking",false);
            playerAnimator.SetBool("isFlying",true);
        }

        if(isGliding){
            verticalSpeed-=gravity * glideFactor * Time.deltaTime;
        }
        else{
            verticalSpeed-=gravity * Time.deltaTime;
        }
        var verticalMove = Vector3.up * verticalSpeed * Time.deltaTime;

        if(!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Eating")){
            if( direction.magnitude != 0){
                this.transform.forward = Vector3.Slerp(this.transform.forward, direction.normalized, rotationSpeed);
            }
            characterController.Move(planeMove + verticalMove);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if(isGrounded){
            if(hit.gameObject.transform.root.CompareTag("Boss")){
                if(this.transform.parent != hit.gameObject.transform.root){
                    this.transform.parent = hit.gameObject.transform.root;
                }
            }
            else{
                this.transform.parent = null;
            }
            if(hit.gameObject.CompareTag("KillBossPlataform")){
                if(Input.GetMouseButtonDown(0)){
                    ChangeKillBossPlataformColor(hit.gameObject); //como setar para precisar ser plataforma?
                }
            }
        }
    }

    void ChangeKillBossPlataformColor(GameObject plataform){
        if(plataform.GetComponent<Renderer>().material.color!=killColor){
            FindObjectOfType<AudioManager>().PlayDelayed("plataformClick");
            plataform.GetComponent<Renderer>().material.color = killColor;
            foreach(Transform child in plataform.transform){
                child.gameObject.SetActive(false);
            }
        }
    }
    public void ResetTurboTimer()
    {
        turboTimer = turboBufferTime;
    }
}
