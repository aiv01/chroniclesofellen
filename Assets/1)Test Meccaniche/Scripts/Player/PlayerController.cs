using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;


namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputMgr))]
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(ShootComponent))]
    public class PlayerController : MonoBehaviour
    {
        #region Components and objects reference
        [Header("Components Reference")]
        private Animator animator;
        private CharacterController characterController;
        private InputMgr input;
        [SerializeField]
        private Staff staff;
        [SerializeField]
        private Transform cameraTransform;
        [SerializeField]
        private GameObject prefabFollow;
        [SerializeField]
        private ShootComponent shootComponent;
        private HealthComponent playerHealth;
        [SerializeField]
        private LayerMask layerMask = new LayerMask();
        [SerializeField]
        private Image crossHair;
        #endregion

        #region Movement variables
        [Header("Movement variables")]
        private Vector3 movement;
        private Vector3 movementOnAim;
        private Vector3 targetDirection;
        private float velocity;
        [SerializeField]
        private float movementSpeed;
        private float rotationTime = 0.1f;
        private float currentAngle;
        private float currentAngleVelocity;
        private bool rotateOnMove = true;
        #endregion

        #region Camera variables
        [Header("Camera variables")]
        [SerializeField]
        private Transform cameraFollowTarget;
        private float xRotation;
        private float yRotation;
        [SerializeField]
        [Range(0.0f, 100f)]
        private float inputSensitivity;
        [SerializeField]
        private CinemachineVirtualCamera aimCamera;
        #endregion

        #region gravity variables
        private float gravity = -9.81f;
        private float groundedGravity = -0.5f;
        [SerializeField]
        private bool isGrounded;
        private Vector3 spherePos;
        [SerializeField]
        private float groundOffsetY;
        #endregion

        #region Jump variables
        [Header("Jump variables")]
        private Vector3 jump;
        private bool isJumping = false;
        [SerializeField]
        private float jumpVelocity = 10f;
        private float counter = 0f;
        #endregion

        #region Attack variables
        [Header("Attack variables")]
        private bool isMeleeReady = false;
        private bool isAttacking = false;
        private int comboCounter = 0;
        private int actualUse = 0;
        [SerializeField]
        private Transform shootTarget;
        private float minDistanceToTarget = 20f;
        private Ray ray;
        #endregion

        #region other
        float timer = 0;
        int randomIdle;
        private bool hasKey;
        public bool HasKey { get { return hasKey; } set { hasKey = value; } }
        #endregion

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            playerHealth = GetComponent<HealthComponent>();
            characterController = GetComponent<CharacterController>();
            input = GetComponent<InputMgr>();
            ray = new Ray(characterController.bounds.center,Vector3.down);
            cameraTransform = Camera.main.transform;
            aimCamera.gameObject.SetActive(false);
            staff.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;

        }

        void Update()
        {

            //IsGrounded();

            prefabFollow.transform.position = transform.position;
            if (!playerHealth.IsAlive)
            {
                animator.SetTrigger("Death");
                return;
            }
            if (input.IsMovementPressed && !input.IsAiming)
            {
                animator.SetBool("IsShootReady", false);
                Movement();

            }
            if (input.IsMovementPressed && input.IsAiming)
            {
                animator.SetBool("IsShootReady", true);
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


        private void Movement()
        {

            movement = new Vector3(input.MovementInput.x, 0, input.MovementInput.y);
            float targetRotation = 0;

            if (movement.magnitude >= 0.1f)
            {

                if (rotateOnMove)
                {

                    targetRotation = Quaternion.LookRotation(movement).eulerAngles.y + cameraTransform.rotation.eulerAngles.y;
                    Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 7 * Time.deltaTime);
                }


            }
            if (!input.IsMovementPressed)
            {
                targetDirection = Vector3.zero;
                movement = Vector3.zero;
                input.MovementInput = Vector2.zero;
                animator.SetFloat("ForwardSpeed", 0f);
            }
            animator.SetFloat("ForwardSpeed", input.MovementInput.magnitude);
            targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
            characterController.Move(targetDirection * movementSpeed * Time.deltaTime);


        }
        private void MovementOnAim()
        {

            movementOnAim = new Vector3(input.MovementInput.x, 0, input.MovementInput.y);
            float targetRotation = 0;

            if (movementOnAim.magnitude >= 0.1f)
            {
                targetRotation = Quaternion.LookRotation(movementOnAim).eulerAngles.y + cameraTransform.rotation.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 7 * Time.deltaTime);

            }
            animator.SetFloat("GunForward", movementOnAim.z);
            animator.SetFloat("GunStrafe", movementOnAim.x);
            Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
            characterController.Move(targetDirection * movementSpeed * Time.deltaTime);

            if (!input.IsMovementPressed)
            {
                targetDirection = Vector3.zero;
                movementOnAim = Vector3.zero;
                input.MovementInput = Vector3.zero;
                animator.SetFloat("GunForward", 0f);
                animator.SetFloat("GunStrafe", 0f);
            }
        }
        private void IsGrounded()
        {
             ray = new Ray(characterController.bounds.center,Vector3.down);
             if(Physics.Raycast(characterController.center,Vector3.down,characterController.bounds.extents.y,layerMask))
             {
                isGrounded = true;
                
             } 
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin,Vector3.down);
            
        }
        private void TimeOutToIdle()
        {

            float maxTimerToIdle = 15f;

            if (input.IsAttackPressed || input.IsMovementPressed || input.IsAiming || input.IsJumpPressed || input.IsShootPressed)
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

        private void Jump()
        {
            if (!isJumping && input.IsJumpPressed)
            {
                isGrounded = false;
                isJumping = true;
                jump.y = jumpVelocity;
               

            }
            else if (isJumping && input.IsJumpPressed && input.InputJumpCounter == 2)
            {
                jump.y = jumpVelocity + (jumpVelocity / 2);
                animator.SetFloat("AirborneVerticalSpeed", jump.y);
                counter += Time.deltaTime;
                if (input.IsJumpPressed && counter >= 0.2f)
                {
                    counter = 0;
                    input.IsJumpPressed = false;
                }
            }
            else if (!input.IsJumpPressed && isJumping && isGrounded)
            {
                isJumping = false;
                input.InputJumpCounter = 0;
            }

            characterController.Move(jump * Time.deltaTime);


        }
        private void ApplyGravity()
        {

            if (!isGrounded)
            {
                jump.y += gravity * Time.deltaTime;
                animator.SetBool("Grounded", false);
                animator.SetFloat("AirborneVerticalSpeed", jump.y);

            }
            else
            {
                jump.y = groundedGravity * Time.deltaTime;
                animator.SetBool("Grounded", true);
            }

        }
        #endregion
        #region Attack Mechanics
        private void MeleeAttack()
        {
            if (input.IsAttackPressed && isGrounded && !isAttacking && staff.gameObject.activeInHierarchy)
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
            else if (!input.IsAttackPressed && isAttacking && isGrounded)
            {

                isAttacking = false;
                animator.SetBool("IsAttacking", false);

            }
            else if (isAttacking && input.IsAttackPressed && comboCounter >= 4)
            {
                comboCounter = 0;
                animator.SetInteger("ComboCounter", comboCounter);
                animator.SetBool("IsAttacking", false);
            }

        }

        private void Aim()
        {
            if (!shootComponent.gameObject.activeInHierarchy) return;
            Vector3 worldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
            {
                shootTarget.position = raycastHit.point;
                worldPosition = raycastHit.point;
            }

            if (input.IsAiming)
            {
                animator.SetBool("IsShootReady", true);
                animator.SetLayerWeight(1, 1f);
                aimCamera.gameObject.SetActive(true);
                crossHair.enabled = true;
                rotateOnMove = false;
                Vector3 aimTarget = worldPosition;
                aimTarget.y = transform.position.y;
                Vector3 aimDirection = (aimTarget - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else
            {
                animator.SetBool("IsShootReady", false);
                animator.SetLayerWeight(1, 0f);
                aimCamera.gameObject.SetActive(false);
                crossHair.enabled = false;
                rotateOnMove = true;
            }



        }

        private void Shoot()
        {
            if (input.IsAiming && input.IsShootPressed && shootComponent.gameObject.activeInHierarchy)
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


        void CameraControl()
        {
            xRotation += input.LookInput.y * Time.deltaTime * inputSensitivity;
            yRotation += input.LookInput.x * Time.deltaTime * inputSensitivity;
            xRotation = Mathf.Clamp(xRotation, -30, 70);
            Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
            cameraFollowTarget.rotation = rotation;
        }
        #endregion


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
