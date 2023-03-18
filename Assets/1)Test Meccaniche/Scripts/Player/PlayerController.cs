using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TheChroniclesOfEllen {

    public class PlayerController : MonoBehaviour
    {
        #region Components and objects reference
        private Animator animator;
        private CharacterController characterController;
        private PlayerInput playerInput;
        [SerializeField]
        private Staff staff;
        public Staff Staff { get { return staff; } set { staff = value; } }
        [SerializeField]
        private Transform cameraTransform;
        [SerializeField]
        private ShootComponent shootComponent;
        private HealthComponent playerHealth;
        
        //reference powerup e vita e danno

        #endregion
        #region Input variables
        private Vector2 movementInput;
        private bool isMovementPressed = false;
        private bool isGrounded = true;
        private bool isJumpPressed = false;
        private bool isJumping = false;
        private bool isAttackPressed = false;
        private bool isAttacking = false;
        private bool isMeleeReady = false;
        public bool IsMeleeReady { get { return isMeleeReady; } set { isMeleeReady = value; } }
        private bool isRangedReady = false;
        private bool isShootPressed = false;
        #endregion
        #region Movement variables
        private Vector3 movement;
        private float velocity;
        [SerializeField]
        private float movementSpeed;
        private float rotationTime = 0.1f;
        private float currentAngle;
        private float currentAngleVelocity;
        #endregion
        #region Camera variables
        private Vector2 lookInput;
        private Vector3 look;
        #endregion
        #region gravity variables
        private float gravity = -9.81f;
        private float groundedGravity = -0.5f;
        private float fallMultiplier = 2.0f;
        #endregion
        #region Jump variables
        private Vector3 jump;
        private float jumpVelocity = 10f;
        private int jumpCounter = 0;
        #endregion
        #region Attack variables
        private int comboCounter = 0;
        private int actualUse = 0;
        #endregion
        #region other
       float timer = 0;
        int randomIdle;
        private bool hasKey;
        public bool HasKey { get { return hasKey; } set { hasKey = value; } }
        #endregion

        void Awake()
        {
            animator = GetComponent<Animator>();
            playerHealth = GetComponent<HealthComponent>();
            characterController = GetComponent<CharacterController>();
            cameraTransform = Camera.main.transform;
            Cursor.lockState = CursorLockMode.Confined;

            staff.gameObject.SetActive(false);
            SetInput();

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
            playerInput.Player.Look.started += onLook;
            playerInput.Player.Look.performed += onLook;
            playerInput.Player.Look.canceled += onLook;
            playerInput.Player.MeleeAttack.started += onMeleeAttack;
            playerInput.Player.MeleeAttack.performed += onMeleeAttack;
            playerInput.Player.MeleeAttack.canceled += onMeleeAttack;
            playerInput.Player.Shoot.started += onShoot;
            playerInput.Player.Shoot.performed += onShoot;
            playerInput.Player.Shoot.canceled += onShoot;
            playerInput.Player.Aim.started += onAim;
            playerInput.Player.Aim.performed += onAim;
            playerInput.Player.Aim.canceled += onAim;

        }

        void Update()
        {
            if (!playerHealth.IsAlive)
            {
                Debug.Log("A pischella � laziale");
                return;
            }
            if (isGrounded)
            {
                jump.y = groundedGravity;
                animator.SetBool("Grounded", true);
            }

            if(isMovementPressed) Movement();
            Look();

            ApplyGravity();
            Jump();

            if (isMeleeReady) MeleeAttack();
            Aim();
            Shoot();

            TimeOutToIdle();


        }
        #region Movement Mechanics
        private void onMovement(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
            if (context.canceled)
            {
                movementInput = Vector2.zero;
                velocity = 0;
                movement = Vector3.zero;
                animator.SetFloat("ForwardSpeed", velocity);
            }

            isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
        }

        private void Movement()
        {
            movement = new Vector3(movementInput.x, 0, movementInput.y);
            if (movement.magnitude >= 0.1f)
            {
                float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                currentAngle = Mathf.SmoothDampAngle(currentAngle, angle, ref currentAngleVelocity, rotationTime);
                Vector3 rotatedMovement = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                transform.rotation = Quaternion.Euler(0, currentAngle, 0);
                characterController.Move(rotatedMovement.normalized * movementSpeed * Time.deltaTime);
                velocity = Vector3.Dot(rotatedMovement.normalized, transform.forward);
                animator.SetFloat("ForwardSpeed", velocity, 0.5f, Time.deltaTime);
            }

            if (movement == Vector3.zero) return;

        }

        private void onLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
            if(context.canceled) lookInput = Vector2.zero;
        }
        private void Look()
        {
          look = new Vector3(lookInput.x,lookInput.y,0);
          float angle = Mathf.Atan2(look.x,look.y) * Mathf.Rad2Deg;
          cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation,transform.rotation,angle * Time.deltaTime);
        }
        private void TimeOutToIdle()
        {

            float maxTimerToIdle = 15f;

            if (isMovementPressed || isJumpPressed || isAttackPressed || isRangedReady || isShootPressed)
            {
                MakeWeaponAppearinInputDetected();
                timer = 0;
                return;

            } else
            {
                timer += Time.deltaTime;

                if (timer >= maxTimerToIdle)
                {
                    animator.SetTrigger("TimeoutToIdle");
                    randomIdle = Random.Range(0, 2);
                    animator.SetInteger("RandomIdle", randomIdle);
                    MakeWeaponDisappearInIdle();

                    timer = 0;
                }

            }
        }
        #endregion
        #region Jump Mechanics
        void onJump(InputAction.CallbackContext context)
        {
            isJumpPressed = context.ReadValueAsButton();
            if (context.started) jumpCounter++;

        }
        private void Jump()
        {
            if (!isJumping && isJumpPressed) {


                isJumping = true;
                isGrounded = false;
                animator.SetBool("Grounded", false);
                jump.y = jumpVelocity;

            } else if (isJumping && isJumpPressed && jumpCounter == 2)
            {
                jump.y = jumpVelocity * 2;
                animator.SetFloat("AirborneVerticalSpeed", jump.y);
            }
            else if (!isJumpPressed && isJumping && isGrounded)
            {
                isJumping = false;
                jumpCounter = 0;
            }
            characterController.Move(jump * Time.deltaTime);
            if (characterController.isGrounded) isGrounded = true;

        }
        private void ApplyGravity()
        {
            if (transform.localPosition.y < -1.0f)
            {
                animator.SetBool("Grounded", false);
                float previousJumpVelocity = jump.y;
                float newJumpVelocity = jump.y + (gravity * fallMultiplier * Time.deltaTime);
                float nextJumpVelocity = (previousJumpVelocity + newJumpVelocity) * 0.5f;
                jump.y = nextJumpVelocity;
                animator.SetFloat("AirborneVerticalSpeed", jump.y);

            } else if (isJumping)
            {
                jump.y += gravity * Time.deltaTime;
                animator.SetFloat("AirborneVerticalSpeed", jump.y);

            }
            if (characterController.isGrounded) isGrounded = true;


        }
        #endregion
        #region Attack Mechanics
        private void onMeleeAttack(InputAction.CallbackContext context)
        {

            isAttackPressed = context.ReadValueAsButton();

        }
        private void MeleeAttack()
        {
            if (isAttackPressed && isGrounded && !isAttacking)
            {
                isAttacking = true;
                movement = Vector3.zero;
                animator.SetBool("IsAttacking", true);
                comboCounter++;
                actualUse++;
                animator.SetInteger("ComboCounter", comboCounter);
                if (comboCounter >= 4)
                {
                    comboCounter = 0;
                    animator.SetInteger("ComboCounter", comboCounter);
                }
                if (actualUse >= staff.MaxUsesSeries)
                {
                    isMeleeReady = false;
                    isAttacking = false;
                    comboCounter = 0;
                    actualUse = 0;
                    staff.gameObject.SetActive(false);
                    animator.SetBool("IsAttacking", isAttacking);
                }


            } else if (!isAttackPressed && isAttacking && isGrounded)
            {

                isAttacking = false;
                animator.SetBool("IsAttacking", false);

            } else if (isAttacking && isAttackPressed && comboCounter >= 4)
            {
                comboCounter = 0;
                animator.SetInteger("ComboCounter", comboCounter);
                animator.SetBool("IsAttacking", false);
            }


        }
        private void onAim(InputAction.CallbackContext context)
        {
            isRangedReady = context.ReadValueAsButton();
        }
        private void Aim()
        {
            if (isRangedReady)
            {
                animator.SetBool("IsShootReady", true);
                animator.SetLayerWeight(1, 1f);
            } else
            {
                animator.SetBool("IsShootReady", false);
                animator.SetLayerWeight(1, 0f);
            }

        }
        private void onShoot(InputAction.CallbackContext context)
        {

            if (context.started)
            {
                isShootPressed = true;


            } else if (context.canceled)
            {
                isShootPressed = false;

            }


        }
        private void Shoot()
        {
            if (isRangedReady && isShootPressed && !isMeleeReady)
            {
                animator.SetBool("IsShooting", true);
                shootComponent.Shoot();
            } 
            else
            {
                animator.SetBool("IsShooting", false);
            }
        }
        #endregion
        //cunzione scudo
        void MakeWeaponDisappearInIdle()
        {
            //control weapon type
            if (staff.gameObject.activeInHierarchy == false) return;
            staff.transform.GetComponentInChildren<Renderer>().enabled = false;
        }
        void MakeWeaponAppearinInputDetected()
        {
            if (staff.gameObject.activeInHierarchy == false) return;
            staff.transform.GetComponentInChildren<Renderer>().enabled = true;
        }
    }
}
