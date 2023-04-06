using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using System.Runtime.InteropServices.WindowsRuntime;


namespace TheChroniclesOfEllen
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputMgr))]
    [RequireComponent(typeof(HealthComponent))]
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
        private ShootComponent gun;
        public HealthComponent playerHealth;
        [SerializeField]
        private Image crossHair;
        private AudioPlayer audioPlayer;
        [SerializeField]
        private Rig aimRig;
        [SerializeField]
        private UnityEvent OnDie;
        #endregion

        #region Movement variables
        [Header("Movement variables")]
        private Vector3 movement;
        private Vector3 movementOnAim;
        private Vector3 targetDirection;
        [SerializeField]
        private float movementSpeed;
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
        private float gamepadInputSensitivity;
        [SerializeField]
        [Range(0.0f, 100f)]
        private float mouseInputSensitivity;

        [SerializeField]
        private CinemachineVirtualCamera aimCamera;
        #endregion

        #region gravity variables
        private float gravity = -9.81f;
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
        private bool hasPickedStaff = false;
        private bool isMeleeReady = false;
        private bool isAttacking = false;
        private int comboCounter = 0;
        private int actualUse = 0;
        [SerializeField]
        private Transform shootTarget;
        #endregion

        #region other
        float timer = 0;
        int randomIdle;
        #endregion


        void Awake()
        {
            animator = GetComponent<Animator>();
            playerHealth = GetComponent<HealthComponent>();
            characterController = GetComponent<CharacterController>();
            gun = GetComponentInChildren<ShootComponent>();
            input = GetComponent<InputMgr>();
            audioPlayer = GetComponent<AudioPlayer>();
            cameraTransform = Camera.main.transform;
            aimCamera.gameObject.SetActive(false);
            staff.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;


        }
        void Update()
        {
            Movement();
            ApplyGravity();
            Jump();
            if (isMeleeReady) MeleeAttack();
            Aim();
            Shoot();
            TimeOutToIdle();
            ChangeWeapon();

            if (!playerHealth.IsAlive)
            {
                Death();
            }

            animator.SetBool("Grounded", characterController.isGrounded);

        }

        void LateUpdate()
        {
            CameraControl();
        }

        #region Movement Mechanics
        private void Movement()
        {

            if (input.IsMovementPressed)
            {
                movement = new Vector3(input.MovementInput.x, 0, input.MovementInput.y);
                float targetRotation = 0;

                targetRotation = Quaternion.LookRotation(movement).eulerAngles.y + cameraTransform.rotation.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);

                targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

                if (input.IsAiming)
                {

                    animator.SetBool("IsShootReady", true);
                    animator.SetFloat("GunForward", movement.z);
                    animator.SetFloat("GunStrafe", movement.x);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 7 * Time.deltaTime);

                }
                else
                {
                    animator.SetBool("IsShootReady", false);
                    animator.SetFloat("ForwardSpeed", input.MovementInput.magnitude);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 7 * Time.deltaTime);
                }
                characterController.Move(targetDirection * movementSpeed * Time.deltaTime);


            }
            else
            {
                targetDirection = Vector3.zero;
                movement = Vector3.zero;
                input.MovementInput = Vector2.zero;
                animator.SetFloat("ForwardSpeed", 0f);
                animator.SetFloat("GunForward", 0f);
                animator.SetFloat("GunStrafe", 0f);
            }


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
                isJumping = true;
                jump.y = jumpVelocity;

            }
            else if (isJumping && input.IsJumpPressed && input.InputJumpCounter == 2)
            {

                jump.y = jumpVelocity + (jumpVelocity / 2);
                counter += Time.deltaTime;

                if (input.IsJumpPressed && counter >= 0.1f)
                {
                    audioPlayer.PlayJumpAudio();
                    counter = 0;
                    input.IsJumpPressed = false;
                }
            }
            else if (!input.IsJumpPressed && isJumping && characterController.isGrounded)
            {
                isJumping = false;
                animator.SetBool("Grounded", characterController.isGrounded);
                input.InputJumpCounter = 0;
            }

            characterController.Move(jump * Time.deltaTime);
            animator.SetFloat("AirborneVerticalSpeed", jump.y);



        }
        private void ApplyGravity()
        {

            if (!characterController.isGrounded)
            {

                jump.y += gravity * Time.deltaTime;
            }



        }
        #endregion
        #region Attack Mechanics
        private void MeleeAttack()
        {
            if (input.IsAttackPressed && characterController.isGrounded && !isAttacking && staff.gameObject.activeInHierarchy)
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
                    hasPickedStaff = false;
                    comboCounter = 0;
                    actualUse = 0;
                    staff.gameObject.SetActive(false);
                    gun.gameObject.SetActive(true);
                    animator.SetBool("IsAttacking", isAttacking);
                }


            }
            else if (!input.IsAttackPressed && isAttacking && characterController.isGrounded)
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

            if (isAttacking)
            {
                staff.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            }
            else
            {
                staff.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            }

        }

        private void Aim()
        {
            if (!gun.gameObject.activeInHierarchy) return;


            Vector3 worldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            Debug.DrawRay(ray.origin, ray.direction, Color.green, Mathf.Infinity);
            LayerMask rayMask = ~(1 << LayerMask.NameToLayer("Player"));

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, rayMask))
            {
                shootTarget.position = raycastHit.point;
                worldPosition = raycastHit.point;
            }

            if (input.IsAiming)
            {
                animator.SetBool("IsShootReady", true);
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
                aimRig.weight = Mathf.Lerp(1f, 0f, Time.deltaTime * 20f);
                aimCamera.gameObject.SetActive(true);
                crossHair.enabled = true;
                rotateOnMove = false;
                Vector3 aimTarget = worldPosition;
                aimTarget.y = transform.position.y;
                Vector3 aimDirection = (aimTarget - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 10f);



            }
            else
            {
                animator.SetBool("IsShootReady", false);
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
                aimRig.weight = Mathf.Lerp(0f, 1f, Time.deltaTime * 20f);
                aimCamera.gameObject.SetActive(false);
                crossHair.enabled = false;
                rotateOnMove = true;
                shootTarget.position = Vector3.zero;
            }



        }

        private void Shoot()
        {
            if (input.IsAiming && input.IsShootPressed && gun.gameObject.activeInHierarchy)
            {
                if (shootTarget != null)
                {
                    gun.OnShoot(shootTarget);

                }
                else return;


            }
        }
        #endregion
        #region Camera Mechanics
        void CameraControl()
        {
            if (input.IsUsingGamepad)
            {
                xRotation += input.LookGamePadInput.y * Time.deltaTime * gamepadInputSensitivity;
                yRotation += input.LookGamePadInput.x * Time.deltaTime * gamepadInputSensitivity;
                xRotation = Mathf.Clamp(xRotation, -30, 70);
                Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
                cameraFollowTarget.rotation = rotation;
            }
            else if (input.IsUsingMouse)
            {
                xRotation += input.LookMouseInput.y * Time.deltaTime * mouseInputSensitivity;
                yRotation += input.LookMouseInput.x * Time.deltaTime * mouseInputSensitivity;
                xRotation = Mathf.Clamp(xRotation, -30, 70);
                Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
                cameraFollowTarget.rotation = rotation;
            }

        }

        #endregion

        void Death()
        {
            animator.SetTrigger("Death");
            playerHealth.currentHealth = 0;
            cameraFollowTarget.parent = null;
            movement = Vector3.zero;
            OnDie.Invoke();
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
            hasPickedStaff = true;
            isMeleeReady = true;
            staff.gameObject.SetActive(status);
            gun.gameObject.SetActive(false);
        }

        void ChangeWeapon()
        {
            if (hasPickedStaff)
            {
                if (input.SwitchWeapon == (int)WeaponType.Pistol)
                {
                    gun.gameObject.SetActive(true);
                    isMeleeReady = false;
                    staff.gameObject.SetActive(false);

                }
                else if (input.SwitchWeapon == (int)WeaponType.Staff)
                {
                    gun.gameObject.SetActive(false);
                    isMeleeReady = true;
                    staff.gameObject.SetActive(true);
                }
            }
            else
            {
                return;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "DeathZone":
                    Death();
                    break;

                case "Platform":
                    transform.parent = transform;
                    break;

                default:
                    return;

            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Platform")
            {

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Platform")
            {
                transform.parent = null;
            }
        }

    }
}
