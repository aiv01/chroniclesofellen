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
   private bool isMovementPressed = false;
  

   //Movement variables
   private Vector3 movement;
   [SerializeField]
   private float movementSpeed;
   private float rotationTime = 0.1f;
   private float currentAngle;
   private float currentAngleVelocity;
   private bool isGrounded = true;
   private float gravity = -9.81f;
   private float groundedGravity = -0.5f;
   private float fallMultiplier = 2.0f;
   
   //Jump variables
   private Vector3 jump;
   private bool isJumpPressed = false;
   private bool isJumping = false;
   private float jumpVelocity = 10f;
   private int jumpCounter = 0;
   

   //attack variables
   private bool isAttackPressed = false;
   private bool isAttacking = false;
   private bool isMeleeReady = false;
   private int comboCounter = 0;
   private float timeForAttack = 0;
   private float maxTimeForAttack = 2f;

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
        Cursor.lockState = CursorLockMode.Confined;

        SetInput();
        staff.SetActive(false);

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
     if(isGrounded)
     {
          jump.y = groundedGravity;
          animator.SetBool("Grounded",true);
     }

     if(isMovementPressed)
     {
          Movement();

     }
          ApplyGravity(); 
          Jump();
          if(isMeleeReady) MeleeAttack();
          TimeOutToIdle();
               
          
   }


   #region Movement Mechanics
   private void onMovement(InputAction.CallbackContext context)
   {
          movementInput = context.ReadValue<Vector2>();
          if(context.canceled)
          {
               movementInput = Vector2.zero;

          }

          isMovementPressed = movementInput.x !=0 || movementInput.y != 0;
   }

   private void Movement()
   {
      movement = new Vector3(movementInput.x,0,movementInput.y);
      if(movement.magnitude >= 0.1f)
      {
          float angle = Mathf.Atan2(movement.x,movement.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
          currentAngle = Mathf.SmoothDampAngle(currentAngle,angle,ref currentAngleVelocity,rotationTime);
          Vector3 rotatedMovement = Quaternion.Euler(0,angle,0) * Vector3.forward;
          transform.rotation = Quaternion.Euler(0,currentAngle,0);
          characterController.Move(rotatedMovement.normalized * movementSpeed * Time.deltaTime);
          float velocity = Vector3.Dot(rotatedMovement.normalized,transform.forward);
          animator.SetFloat("ForwardSpeed",velocity,0.5f,Time.deltaTime);
      }
     
      if(movement == Vector3.zero) return;
      
      

   }

   
   #endregion


   #region Jump Mechanics
   void onJump(InputAction.CallbackContext context)
   { 
          isJumpPressed = context.ReadValueAsButton();
         if(context.started) jumpCounter++;
          
   }
   private void Jump()
   {
     if(!isJumping  && isJumpPressed){

          
          isJumping = true;
          isGrounded = false;
          animator.SetBool("Grounded",false);
          jump.y = jumpVelocity;
          
     }else if(isJumping && isJumpPressed && jumpCounter == 2)
     {
          jump.y = jumpVelocity * 2;
          animator.SetFloat("AirborneVerticalSpeed",jump.y);
     }
     else if(!isJumpPressed && isJumping && isGrounded)
     {
               isJumping = false;
               jumpCounter = 0;
     }
          characterController.Move(jump * Time.deltaTime);
          if(characterController.isGrounded) isGrounded = true;
     
   }
   private void ApplyGravity()
   {
     if(transform.localPosition.y < -1.0f)
     {
          animator.SetBool("Grounded",false);
          float previousJumpVelocity = jump.y;
          float newJumpVelocity = jump.y + (gravity * fallMultiplier * Time.deltaTime);
          float nextJumpVelocity = (previousJumpVelocity + newJumpVelocity) * 0.5f;
          jump.y = nextJumpVelocity;
          animator.SetFloat("AirborneVerticalSpeed",jump.y);

     }else if(isJumping)
     {
          jump.y += gravity * Time.deltaTime;
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
          movement = Vector3.zero;
          animator.SetBool("IsAttacking",true);
          comboCounter ++;
          animator.SetInteger("ComboCounter",comboCounter);
          if(comboCounter >= 4)
          {
               comboCounter = 0;
               animator.SetInteger("ComboCounter",comboCounter);
          }

      }else if(!isAttackPressed && isAttacking && isGrounded)
      {

               isAttacking = false;
               animator.SetBool("IsAttacking",false);

      }else if(isAttacking && isAttackPressed && comboCounter >=4)
      {
               comboCounter = 0;
               animator.SetInteger("ComboCounter",comboCounter);
               animator.SetBool("IsAttacking",false);
      }
        
     
    } 
    private void TimeOutToIdle()
    {
      
      float maxTimerToIdle = 15f;

      if(isMovementPressed || isJumpPressed || isAttackPressed)
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
