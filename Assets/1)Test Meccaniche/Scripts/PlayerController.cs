using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace TheChroniclesOfEllen{

public class PlayerController : MonoBehaviour
{
   private Animator animator;
   private CharacterController characterController;
   private PlayerInput playerInput;
   [SerializeField]
   private CinemachineFreeLook cinemachineCamera;
   private Vector2 movementInput;
   private bool isMovementPressed = false;
   private Vector3 movement;
   [SerializeField]
   private float movementSpeed;
   [SerializeField]
   Quaternion targetRotation;
   private float smoothAngle = .5f;
   private float rotationVelocity;
   private float rotationSpeed = 5f;

   private bool isGrounded = true;
   private float gravity = -9.81f;
   private float groundedGravity = -0.5f;
   private float fallMultiplier = 2.0f;
   
   //Jump variables
   private Vector3 jump;
   private float jumpSpeed = 5f;
   private bool isJumpPressed = false;
   private bool isJumping = false;
   private float jumpVelocity;
   private float maxJumpHeight = 6f;
   private float maxJumpTime = .75f;
   

   //attack variables
   private bool isAttackPressed = false;
   private bool isAttacking = false;
   private float stateTime = 0;

   void Awake()
   {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        playerInput = new PlayerInput();
        playerInput.Player.Jump.started += onJump;
        playerInput.Player.Jump.canceled += onJump;
        playerInput.Player.Movement.started += onMovement;
        playerInput.Player.Movement.performed += onMovement;
        playerInput.Player.Movement.canceled += onMovement;
        playerInput.Player.MeleeAttack.started +=onMeleeAttack;
        playerInput.Player.MeleeAttack.performed +=onMeleeAttack;
        playerInput.Player.MeleeAttack.canceled +=onMeleeAttack;

        
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex,2);
        jumpVelocity = (2 * maxJumpHeight) / timeToApex;
   }
    void OnEnable()
   {
        playerInput.Enable();
   }
   
      void OnDisable()
   {
        playerInput.Disable();
   }
 
    void Start()
    {
          isGrounded = true;
          jump.y = groundedGravity;
          animator.SetBool("Grounded",true);
          
    }
 
   void Update()
   {

     if(isMovementPressed)
     {
         characterController.Move(movement.normalized * movementSpeed * Time.deltaTime);
         animator.SetFloat("ForwardSpeed",movement.z);
         
         Vector3 forwardDirection = Quaternion.Euler(0,cinemachineCamera.m_XAxis.Value,0) * Vector3.forward;
         forwardDirection.y = 0;
         forwardDirection.Normalize();
         
         
         if(Mathf.Approximately(Vector3.Dot(movement.normalized,Vector3.forward),-1f))
         {
               targetRotation = Quaternion.LookRotation(-forwardDirection);
         }else
         {
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, movement);
                targetRotation = Quaternion.LookRotation(cameraToInputOffset * forwardDirection);
            
         }

         Vector3 result = targetRotation * Vector3.forward;

            transform.rotation = Quaternion.LookRotation(result);
         
         
     }
     if(isGrounded == false)
     {
          ApplyGravity();
     }
     else
     {
          jump.y = groundedGravity;
          animator.SetBool("Grounded",true);
     }
          
          Jump();
          MeleeAttack();
        
   }
   #region Movement Mechanics
   private void onMovement(InputAction.CallbackContext context)
   {
     movementInput = context.ReadValue<Vector2>();
     movementInput.Normalize();
     movement.x = movementInput.x;
     movement.z = movementInput.y;
     
     isMovementPressed = movementInput.x !=0 || movementInput.y != 0;
   }

   #endregion
   #region Jump Mechanics
   void onJump(InputAction.CallbackContext context)
   { 
     isJumpPressed = context.ReadValueAsButton();
   }

   private void Jump()
   {
     if(!isJumping && isGrounded && isJumpPressed)
     {
          isJumping = true;
          isGrounded = false;
          animator.SetBool("Grounded",isGrounded);
          jump.y = jumpVelocity;
          
          animator.SetFloat("AirborneVerticalSpeed",jump.y);
     }
     else if(!isJumpPressed && isJumping && isGrounded)
     {
          isJumping = false;
          
     }
     characterController.Move(jump * Time.deltaTime);
     if(characterController.isGrounded) isGrounded = true;
     
   }
   private void ApplyGravity()
   {

     if(isJumping)
     {
          animator.SetBool("Grounded",false);
          float previousJumpVelocity = jump.y;
          float newJumpVelocity = jump.y + (gravity * Time.deltaTime);
          float nextJumpVelocity = (previousJumpVelocity + newJumpVelocity) * 0.5f;
          jump.y = nextJumpVelocity;
          animator.SetFloat("AirborneVerticalSpeed",jump.y);

     }else
     {
          animator.SetBool("Grounded",false);
          float previousJumpVelocity = jump.y;
          float newJumpVelocity = jump.y + (gravity * fallMultiplier * Time.deltaTime);
          float nextJumpVelocity = (previousJumpVelocity + newJumpVelocity) * 0.5f;
          jump.y = nextJumpVelocity;
          animator.SetFloat("AirborneVerticalSpeed",jump.y);
     }
     if(characterController.isGrounded) isGrounded = true;
     
   }
   #endregion
   #region Attack Mechanics

    private void onMeleeAttack(InputAction.CallbackContext context)
    {
          isAttackPressed = context.ReadValueAsButton();
          
    }
    private void MeleeAttack()
    {
     if(isAttackPressed && isGrounded && !isAttacking)
      {
          isAttacking = true;
          //animator.SetTrigger("MeleeAttack");
          //StartCoroutine(MeleeAttackCoroutine());
      }else if(!isAttackPressed && isAttacking && isGrounded)
      {
          isAttacking = false;
      }
    } 

   #endregion 
     void OnCollisonExit(Collider other)
     {
          if(other.gameObject.tag == "Ground")
          {
               isGrounded = false;
               Debug.Log("is Falling");
          } 
     }
}
}
