using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheChroniclesOfEllen{

public class PlayerController : MonoBehaviour
{
   private Animator animator;
   private CharacterController characterController;
   private PlayerInput playerInput;
   private Vector2 movementInput;
   private bool isMovementPressed = false;
   private Vector3 movement;
   [SerializeField]
   private float movementSpeed;
   
   private bool isGrounded = true;
   private float gravity = -9.81f;
   private float groundedGravity = -0.5f;
   private float fallMultiplier = 2.0f;
   
   //Jump variables
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
          movement.y = groundedGravity;
          animator.SetBool("Grounded",true);
          
    }
 
   void Update()
   {

     if(isMovementPressed || isJumpPressed)
     {
         characterController.Move(movement.normalized * movementSpeed * Time.deltaTime);
         animator.SetFloat("ForwardSpeed",movement.z);
         
         
     }
     if(isGrounded == false)
     {
          ApplyGravity();
     }
     else
     {
          movement.y = groundedGravity;
          animator.SetBool("Grounded",true);
     }
          
          Jump();
          MeleeAttack();
          
     
        
   }

   private void onMovement(InputAction.CallbackContext context)
   {
     movementInput = context.ReadValue<Vector2>();
     movement.x = movementInput.x;
     movement.z = movementInput.y;
     isMovementPressed = movementInput.x !=0 || movementInput.y != 0;
   }
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
          movement.y = jumpVelocity;
          animator.SetFloat("AirborneVerticalSpeed",jumpVelocity);
     }
     else if(!isJumpPressed && isJumping && isGrounded)
     {
          isJumping = false;
          
     }
   }
   private void ApplyGravity()
   {

     if(isJumping)
     {
          animator.SetBool("Grounded",false);
          float previousJumpVelocity = movement.y;
          float newJumpVelocity = movement.y + (gravity * Time.deltaTime);
          float nextJumpVelocity = (previousJumpVelocity + newJumpVelocity) * 0.5f;
          movement.y = nextJumpVelocity;
          animator.SetFloat("AirborneVerticalSpeed",movement.y);

     }else
     {
          animator.SetBool("Grounded",false);
          float previousJumpVelocity = movement.y;
          float newJumpVelocity = movement.y + (gravity * fallMultiplier * Time.deltaTime);
          float nextJumpVelocity = (previousJumpVelocity + newJumpVelocity) * 0.5f;
          movement.y = nextJumpVelocity;
          animator.SetFloat("AirborneVerticalSpeed",movement.y);
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

}
}
