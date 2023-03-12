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
   
   private float gravity = -9.81f;
   private float groundedGravity = -0.5f;
   private float gravityMultiplier = 3f;
   private float jumpRange = 0;
   //Jump variables
   private bool isJumpPressed = false;
   private bool isJumping = false;
   private float jumpVelocity;
   private float maxJumpHeight = 10f;
   private float maxJumpTime = .55f;



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
      animator.SetBool("Grounded",true);
    }
 
   void Update()
   {
     if(isMovementPressed)
     {
         characterController.Move(movement.normalized * movementSpeed * Time.deltaTime);
         animator.SetFloat("ForwardSpeed",movement.z);
     }
        
        ApplyGravity();
        Jump();
   }
   private void onMovement(InputAction.CallbackContext context)
   {
     movementInput = context.ReadValue<Vector2>();
     movement.x = movementInput.x;
     movement.z = movementInput.y;
     isMovementPressed = movementInput.x !=0 || movement.y != 0;
   }
   void onJump(InputAction.CallbackContext context)
   {   
     isJumpPressed = context.ReadValueAsButton();
   }

   private void Jump()
   {
     if(!isJumping && isGrounded() && isJumpPressed)
     {
          movement.y = jumpVelocity * 0.5f;
          animator.SetBool("Grounded",false);
          isJumping = true;
          

     }else if(!isJumpPressed && isJumping && isGrounded())
     {
          isJumping = false;
     }
   }
   private void ApplyGravity()
   {
       bool isFalling = movement.y <= 0.0f || !isJumpPressed;
       float fallMultiplier = 2.0f;
       if(isGrounded())
       {
          movement.y = groundedGravity;
          animator.SetBool("Grounded",true);
       }else if(isFalling)
       {
          float previousVelocity = movement.y;
          float newVelocity = movement.y + (gravity * fallMultiplier * Time.deltaTime);
          float nextVelocity = (previousVelocity + newVelocity) * 0.5f;
          movement.y = nextVelocity;
          float fall = 0;
          fall -= 2 * Time.deltaTime;
          if(fall <= -1) fall =-1;
          animator.SetFloat("AirborneVerticalSpeed",fall);

       }else
       {
          float previousVelocity = movement.y;
          float newVelocity = movement.y + (gravity * Time.deltaTime);
          float nextVelocity = (previousVelocity + newVelocity) * 0.5f;
          movement.y = nextVelocity;
       }
   }
   private bool isGrounded() => characterController.isGrounded;
   

}
}