using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace TheChroniclesOfEllen{

public class PlayerController : MonoBehaviour
{
   //Component variables
   private Animator animator;
   private CharacterController characterController;
   private PlayerInput playerInput;

   //Input variables
   private Vector2 movementInput;
   private bool inputDetected = false;
   private bool isMovementPressed = false;
  

   //Movement variables
   private Vector3 movement;
   [SerializeField]
   private float movementSpeed;
   private float rotationSpeed = 3f;
   [SerializeField]
   private bool isGrounded = true;
   private float gravity = -9.81f;
   private float groundedGravity = -0.5f;
   private float fallMultiplier = 2.0f;
   private float smoothDamp = 0.5f;
   
   //Jump variables
   private Vector3 jump;
   private bool isJumpPressed = false;
   private bool isJumping = false;
   private float jumpVelocity;
   private float maxJumpHeight = 2f;
   private float maxJumpTime = .75f;
   

   //attack variables
   private bool isAttackPressed = false;
   private bool isAttacking = false;
   private bool isMeleeReady = false;
   private int comboCounter = 0;

   //miscellaneous
   float timer = 0;
   int randomIdle;
   [SerializeField]
   private GameObject staff;
   [SerializeField]
   private Transform cameraTransform;

   void Awake()
   {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        SetInput();
  
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

   private void SetInput()
   {
        playerInput = new PlayerInput();
        playerInput.Player.Jump.started += onJump;
        playerInput.Player.Jump.canceled += onJump;
        playerInput.Player.Movement.started += onMovement;
        playerInput.Player.Movement.performed += onMovement;
        playerInput.Player.Movement.canceled += onMovement;
        playerInput.Player.MeleeAttack.started +=onMeleeAttack;
        playerInput.Player.MeleeAttack.performed +=onMeleeAttack;
        playerInput.Player.MeleeAttack.canceled +=onMeleeAttack;
   }
 
   void Update()
   {

     if(isMovementPressed)
     {
          Movement();

     }else
     {
          inputDetected = false;
          movement = Vector3.zero;
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

          if(isMeleeReady) MeleeAttack();

          TimeOutToIdle();
               
          
   }


   #region Movement Mechanics
   private void onMovement(InputAction.CallbackContext context)
   {
          movementInput = context.ReadValue<Vector2>();
          inputDetected = true;
          isMovementPressed = movementInput.x !=0 || movementInput.y != 0;
   }

   private void Movement()
   {
      Vector3 cameraForward = new Vector3(cameraTransform.forward.x,0,cameraTransform.forward.z);
      Vector3 cameraRight = new Vector3(cameraTransform.right.x,0,cameraTransform.right.z);
      Vector3 moveDirection = cameraForward.normalized * movementInput.y + cameraRight.normalized * movementInput.x;
      movement.x = moveDirection.x * movementSpeed;
      movement.z = moveDirection.z * movementSpeed;
      Vector3 faceDirection = new Vector3(movement.x,0,movement.z);
      if(faceDirection == Vector3.zero) return;
      Quaternion targetRotation = Quaternion.LookRotation(faceDirection);
      transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed* Time.deltaTime);
      if(movement == Vector3.zero) return;
      characterController.Move(movement.normalized * movementSpeed * Time.deltaTime);
      animator.SetFloat("ForwardSpeed",movement.z);
      animator.SetFloat("RightSpeed",movement.x);

   }

   
   #endregion


   #region Jump Mechanics
   void onJump(InputAction.CallbackContext context)
   { 
          isJumpPressed = context.ReadValueAsButton();
          inputDetected = context.ReadValueAsButton();
          
   }
   private void Jump()
   {
     if(!isJumping && isGrounded && isJumpPressed){

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
          inputDetected = context.ReadValueAsButton();
          
    }
    private void MeleeAttack()
    {
     if(isAttackPressed && isGrounded && !isAttacking)
      {
          isAttacking = true;
          comboCounter ++;
          if(comboCounter >= 4)
          {
               comboCounter = 0;
          }
          animator.SetTrigger("MeleeAttack");
          animator.SetInteger("ComboCounter",comboCounter);
     
      }else if(!isAttackPressed && isAttacking && isGrounded)
      {
          isAttacking = false;
          inputDetected = false;
          animator.ResetTrigger("MeleeAttack");
          
      }
     
    } 
    private void TimeOutToIdle()
    {
      
      float maxTimerToIdle = 15f;

      if(inputDetected)
      {
          MakeWeaponAppearinInputDetected();
          timer = 0;
          return;
      
      }else
      {
          timer += Time.deltaTime;

      if(timer >= maxTimerToIdle )
      {
          animator.SetTrigger("TimeoutToIdle");
          randomIdle = Random.Range(0,2);
          animator.SetInteger("RandomIdle",randomIdle);
          MakeWeaponDisappearInIdle();
          
          timer = 0;
      }
          
      }
    }

   #endregion 

     void OnTriggerEnter(Collider other)
     {
          if(other.gameObject.tag == "Weapon")
          {
               staff.SetActive(true);
               isMeleeReady = true;
          }

     }
     void MakeWeaponDisappearInIdle()
     {
          //control weapon type
          if(staff.activeInHierarchy == false) return;
          staff.transform.GetComponentInChildren<Renderer>().enabled = false;
     }
     void MakeWeaponAppearinInputDetected()
     {
          if(staff.activeInHierarchy == false) return;
          staff.transform.GetComponentInChildren<Renderer>().enabled = true;
     }
}
}
