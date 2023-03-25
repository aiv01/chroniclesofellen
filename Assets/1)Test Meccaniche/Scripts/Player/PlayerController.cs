using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.Android;


namespace TheChroniclesOfEllen
{

    public class PlayerController : MonoBehaviour
    {
        #region Components and objects reference
        [Header("Components Reference")]
        private Animator animator;
        private CharacterController characterController;
        private PlayerInput playerInput;
        [SerializeField]
        private Staff staff;
        [SerializeField]
        private Transform cameraTransform;
        [SerializeField]
        private GameObject prefabFollow;
        [SerializeField]
        private CinemachineVirtualCamera playerCamera;
        [SerializeField]
        private CinemachineVirtualCamera aimCamera;
        [SerializeField]
        private ShootComponent shootComponent;
        private HealthComponent playerHealth;
        #endregion

        #region Input variables
        [Header("Input variables")]
        private Vector2 movementInput;
        private bool isMovementPressed = false;
        private bool isGrounded = true;
        private bool isJumpPressed = false;
        private bool isJumping = false;
        private bool isAttackPressed = false;
        private bool isAttacking = false;
        private bool isMeleeReady = false;
        private bool isRangedReady = false;
        private bool isShootPressed = false;
        #endregion

        #region Movement variables
        [Header("Movement variables")]
        private Vector3 movement;
        private Vector3 movementOnAim;
        private float velocity;
        [SerializeField]
        private float movementSpeed;
        private float rotationTime = 0.1f;
        private float currentAngle;
        private float currentAngleVelocity;
        #endregion

        #region Camera variables
        [Header("Camera variables")]
        [SerializeField]
        private Transform cameraFollowTarget;
        private Vector2 lookInput;
        private float xRotation;
        private float yRotation;
        #endregion

        #region gravity variables
        [Header("Gravity variables")]
        private float gravity = -9.81f;
        private float groundedGravity = -0.5f;
        private float fallMultiplier = 2.0f;
        #endregion

        #region Jump variables
        [Header("Jump varialbles")]
        private Vector3 jump;
        [SerializeField]
        private float jumpVelocity = 10f;
        private int jumpCounter = 0;
        private float counter = 0f;
        #endregion

        #region Attack variables
        [Header("Attack variables")]
        private int comboCounter = 0;
        private int actualUse = 0;
        [SerializeField]
        private Transform shootTarget;
        private float minDistanceToTarget = 20f;
        #endregion
        
        #region other
        [Header("Other")]
        float timer = 0;
        int randomIdle;
        private bool hasKey;
        public bool HasKey { get { return hasKey; } set { hasKey = value; } }
        private RaycastHit hitInfo;
        #endregion

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            playerHealth = GetComponent<HealthComponent>();
            characterController = GetComponent<CharacterController>();
            cameraTransform = Camera.main.transform;
            aimCamera.gameObject.SetActive(false);
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
            playerInput.Player.MeleeAttack.started += onMeleeAttack;
            playerInput.Player.MeleeAttack.performed += onMeleeAttack;
            playerInput.Player.MeleeAttack.canceled += onMeleeAttack;
            playerInput.Player.Shoot.started += onShoot;
            playerInput.Player.Shoot.performed += onShoot;
            playerInput.Player.Shoot.canceled += onShoot;
            playerInput.Player.Aim.started += onAim;
            playerInput.Player.Aim.performed += onAim;
            playerInput.Player.Aim.canceled += onAim;
            playerInput.Player.Makeaction.started += onChangeWeapon;
            playerInput.Player.Makeaction.performed += onChangeWeapon;
            playerInput.Player.Makeaction.canceled += onChangeWeapon;
            playerInput.Player.Look.started += onCameraControl;
            playerInput.Player.Look.performed += onCameraControl;
            playerInput.Player.Look.canceled += onCameraControl;



        }
        void Update()
        {
            
            prefabFollow.transform.position = transform.position;
            
            if (!playerHealth.IsAlive)
            {
                Debug.Log("A pischella � laziale");
                animator.SetTrigger("Death");
                return;
            }

            IsGrounded();
            if (isMovementPressed && !isRangedReady)
            {
                animator.SetBool("IsShootReady", isRangedReady);
                Movement();
                
            }
            if (isMovementPressed && isRangedReady)
            {
                animator.SetBool("IsShootReady", isRangedReady);
                MovementOnAim();
            }
            ApplyGravity();
            Jump();
            if (isMeleeReady) MeleeAttack();
            Aim();
            Shoot();
            CameraControl();
            TimeOutToIdle();


        }

        void LateUpdate()
        {
           CameraControl();
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
                movementOnAim = Vector3.zero;
                animator.SetFloat("ForwardSpeed", velocity);
            }

            isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
        }

        private void Movement()
        {

            movement = new Vector3(movementInput.x, 0, movementInput.y);
            float targetRotation = 0;

            if (movement.magnitude >= 0.1f)
            {
                targetRotation = Quaternion.LookRotation(movement).eulerAngles.y + cameraTransform.rotation.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0,targetRotation,0);
                transform.rotation = Quaternion.Slerp(transform.rotation,rotation, 7 * Time.deltaTime);
                
            }
                animator.SetFloat("ForwardSpeed", movementInput.magnitude);
                Vector3 targetDirection = Quaternion.Euler(0,targetRotation,0) * Vector3.forward;
                characterController.Move(targetDirection * movementSpeed * Time.deltaTime);
                
        }
        private void MovementOnAim()
        {

            movementOnAim = new Vector3(movementInput.x, 0, movementInput.y);
            characterController.Move(movementOnAim * movementSpeed * Time.deltaTime);
            animator.SetFloat("GunForward", movementOnAim.z);
            animator.SetFloat("GunStrafe", movementOnAim.x);
            float targetRotation = 0;

            if (movementOnAim.magnitude >= 0.1f)
            {
                targetRotation = Quaternion.LookRotation(movementOnAim).eulerAngles.y + cameraTransform.rotation.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0,targetRotation,0);
                transform.rotation = Quaternion.Slerp(transform.rotation,rotation, 7 * Time.deltaTime);
                
            }
                animator.SetFloat("ForwardSpeed", movementInput.magnitude);
                Vector3 targetDirection = Quaternion.Euler(0,targetRotation,0) * Vector3.forward;
                characterController.Move(targetDirection * movementSpeed * Time.deltaTime);
        }
        private bool IsGrounded()
        {
            if (Physics.Raycast(characterController.bounds.center,-Vector3.up,out hitInfo,characterController.bounds.extents.y+0.1f))
            {
                return isGrounded = true;
            }
            else
            {
                return isGrounded = false;
            }
        }
        private void TimeOutToIdle()
        {

            float maxTimerToIdle = 15f;

            if (isMovementPressed || isJumpPressed || isAttackPressed || isRangedReady || isShootPressed)
            {
                MakeWeaponAppearinInputDetected();
                timer = 0;
                return;

            }
            else
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
            if (!isJumping && isJumpPressed)
            {
                isJumping = true;
                jump.y = jumpVelocity;

            }
            else if (isJumping && isJumpPressed && jumpCounter == 2)
            {
                jump.y = jumpVelocity + (jumpVelocity / 2);
                animator.SetFloat("AirborneVerticalSpeed", jump.y);
                counter += Time.deltaTime;
                if(isJumpPressed && counter >= 0.2f)
                {
                    counter = 0;
                    isJumpPressed = false;
                }
            }
            else if (!isJumpPressed && isJumping && isGrounded)
            {
                isJumping = false;
                jumpCounter = 0;
            }
            characterController.Move(jump * Time.deltaTime);


        }
        private void ApplyGravity()
        {

            if (!isGrounded)
            {
                jump.y += gravity * Time.deltaTime;
                animator.SetBool("Grounded", isGrounded);
                animator.SetFloat("AirborneVerticalSpeed", jump.y);

            }
            else
            {
                jump.y = groundedGravity * Time.deltaTime;
                animator.SetBool("Grounded", isGrounded);
            }

        }
        #endregion
        #region Attack Mechanics
        private void onMeleeAttack(InputAction.CallbackContext context)
        {

            isAttackPressed = context.ReadValueAsButton();

        }
        private void MeleeAttack()
        {
            if (isAttackPressed && isGrounded && !isAttacking && staff.gameObject.activeInHierarchy)
            {
                isAttacking = true;
                movement = Vector3.zero;
                animator.SetBool("IsAttacking", true);
                comboCounter++;

                animator.SetInteger("ComboCounter", comboCounter);
                if (comboCounter >= 4)
                {
                    actualUse++;
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
                    shootComponent.gameObject.SetActive(true);
                    animator.SetBool("IsAttacking", isAttacking);
                }


            }
            else if (!isAttackPressed && isAttacking && isGrounded)
            {

                isAttacking = false;
                animator.SetBool("IsAttacking", false);

            }
            else if (isAttacking && isAttackPressed && comboCounter >= 4)
            {
                comboCounter = 0;
                animator.SetInteger("ComboCounter", comboCounter);
                animator.SetBool("IsAttacking", false);
            }

        }
        private void onAim(InputAction.CallbackContext context)
        {
            if (shootComponent.gameObject.activeInHierarchy)
            {
                isRangedReady = context.ReadValueAsButton();
            }
        }
        private void Aim()
        {
            if (isRangedReady)
            {
                animator.SetBool("IsShootReady", true);
                animator.SetLayerWeight(1, 1f);

                var target = GameObject.FindObjectsByType<BaseEnemyComponent>(FindObjectsSortMode.None);

                foreach (var tar in target)
                {
                    if (tar == null) return;
                    if (Vector3.Distance(transform.position, tar.transform.position) <= minDistanceToTarget)
                    {

                        shootTarget.SetParent(tar.transform);
                        shootTarget.transform.localPosition = Vector3.zero;
                        playerCamera.gameObject.SetActive(false);
                        aimCamera.gameObject.SetActive(true);

                    }
                    else
                    {
                        shootTarget.parent = null;
                        aimCamera.gameObject.SetActive(false);
                        playerCamera.gameObject.SetActive(true);
                    }

                    if (shootTarget.parent != null)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(shootTarget.position - transform.position);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 2f);
                    }

                }

            }
            else
            {
                animator.SetBool("IsShootReady", false);
                animator.SetLayerWeight(1, 0f);
                aimCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
            }

        }
        private void onShoot(InputAction.CallbackContext context)
        {

            if (context.started)
            {
                isShootPressed = true;


            }
            else if (context.canceled)
            {
                isShootPressed = false;

            }

        }
        private void Shoot()
        {
            if (isRangedReady && isShootPressed && shootComponent.gameObject.activeInHierarchy)
            {
                animator.SetBool("IsShooting", true);
                shootComponent.Shoot(shootTarget);
            }
            else
            {
                animator.SetBool("IsShooting", false);
            }
        }
        #endregion
        #region Camera Mechanics
        void onCameraControl(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();
        }

        void CameraControl()
        {
            
            xRotation += lookInput.y;
            yRotation += lookInput.x;
            xRotation = Mathf.Clamp(xRotation,-30,70);
            Quaternion rotation = Quaternion.Euler(xRotation,yRotation,0);
            cameraFollowTarget.rotation = rotation;
        }
        #endregion

        void onChangeWeapon(InputAction.CallbackContext context)
        {
            if (context.started && shootComponent.gameObject.activeInHierarchy && isMeleeReady)
            {
                shootComponent.gameObject.SetActive(false);
                staff.gameObject.SetActive(true);

            }
            else if (context.started && staff.gameObject.activeInHierarchy)
            {
                shootComponent.gameObject.SetActive(true);
                staff.gameObject.SetActive(false);
            }
        }
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

        public void SetStaffStatus(bool status)
        {
            if (isMeleeReady) return;
            isMeleeReady = true;
            staff.gameObject.SetActive(status);
            shootComponent.gameObject.SetActive(false);
        }

    }
}
