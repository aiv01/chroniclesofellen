using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheChroniclesOfEllen
{
    public class InputMgr : MonoBehaviour
    {
        private PlayerInput playerInput;
        private Vector2 movementInput;
        private Vector2 lookGamepadInput;
        private Vector2 lookMouseInput;
        private bool isUsingGamepad = false;
        private bool isUsingMouse = false;
        private bool isMovementPressed = false;
        private bool isJumpPressed = false;
        private bool isAttackPressed = false;
        private bool isAiming = false;
        private bool isShootPressed = false;
        private float inputJumpCounter = 0;
        private int switchWeapon = 0;

        #region Properties
        public Vector2 MovementInput
        {
            get { return movementInput; }
            set { movementInput = value; }
        }
        public Vector2 LookGamePadInput
        {
            get { return lookGamepadInput; }
            set { lookGamepadInput = value; }
        }
        public Vector2 LookMouseInput
        {
            get {return lookMouseInput;}
            set {lookMouseInput = value;}
        }
        public bool IsUsingGamepad
        {
            get{return isUsingGamepad;}
            set{isUsingGamepad = value;}
        }
        public bool IsUsingMouse
        {
            get{return isUsingMouse;}
            set{isUsingMouse = value;}
        }
        public bool IsMovementPressed
        {
            get { return isMovementPressed; }
            set { isMovementPressed = value; }
        }
        public bool IsJumpPressed
        {
            get { return isJumpPressed; }
            set { isJumpPressed = value; }
        }
        public bool IsAttackPressed
        {
            get { return isAttackPressed; }
            set { isAttackPressed = value; }
        }
        public bool IsAiming
        {
            get { return isAiming; }
            set { isAiming = value; }
        }
        public bool IsShootPressed
        {
            get { return isShootPressed; }
            set { isShootPressed = value; }
        }
        public float InputJumpCounter
        {
            get { return inputJumpCounter; }
            set { inputJumpCounter = value; }
        }
        public int SwitchWeapon
        {
            get { return switchWeapon; }
        }
        #endregion

        void Awake()
        {
            playerInput = new PlayerInput();
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

            playerInput.Player.Movement.started += onMovement;
            playerInput.Player.Movement.performed += onMovement;
            playerInput.Player.Movement.canceled += onMovement;
            playerInput.Player.LookGamePad.started += onCameraControl;
            playerInput.Player.LookGamePad.performed += onCameraControl;
            playerInput.Player.LookGamePad.canceled += onCameraControl;
            playerInput.Player.LookMouse.started += onCameraControl;
            playerInput.Player.LookMouse.performed += onCameraControl;
            playerInput.Player.LookMouse.canceled += onCameraControl;
            playerInput.Player.Jump.started += onJump;
            playerInput.Player.Jump.canceled += onJump;
            playerInput.Player.MeleeAttack.started += onMeleeAttack;
            playerInput.Player.MeleeAttack.performed += onMeleeAttack;
            playerInput.Player.MeleeAttack.canceled += onMeleeAttack;
            playerInput.Player.Aim.started += onAim;
            playerInput.Player.Aim.performed += onAim;
            playerInput.Player.Aim.canceled += onAim;
            playerInput.Player.Shoot.started += onShoot;
            playerInput.Player.Shoot.performed += onShoot;
            playerInput.Player.Shoot.canceled += onShoot;
            playerInput.Player.Makeaction.started += onChangeWeapon;
            playerInput.Player.Makeaction.performed += onChangeWeapon;
            playerInput.Player.Makeaction.canceled += onChangeWeapon;

        }

        private void onMovement(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
            if (context.canceled)
            {
                movementInput = Vector2.zero;
            }

            isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
        }
        void onCameraControl(InputAction.CallbackContext context)
        {
            lookGamepadInput = context.ReadValue<Vector2>();
            lookMouseInput = context.ReadValue<Vector2>();

            isUsingGamepad = lookGamepadInput.x != 0 || lookGamepadInput.y != 0;
            isUsingMouse = lookMouseInput.x != 0 || lookMouseInput.y != 0;
            
        }
        void onJump(InputAction.CallbackContext context)
        {

            if (context.started)
            {
                isJumpPressed = true;
                inputJumpCounter++;

            }
            else if (context.canceled)
            {
                isJumpPressed = false;
            }

        }
        private void onMeleeAttack(InputAction.CallbackContext context)
        {

            isAttackPressed = context.ReadValueAsButton();

        }
        private void onAim(InputAction.CallbackContext context)
        {

            isAiming = context.ReadValueAsButton();

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
        void onChangeWeapon(InputAction.CallbackContext context)
        {
            if (switchWeapon < 1)
            {
                switchWeapon = 1;

            }
            else if (switchWeapon > 2)
            {
                switchWeapon = 2;
            }
            if (context.started && switchWeapon == 1)
            {
                switchWeapon += 1;

            }else if (switchWeapon == 2 && context.started)
            {
                switchWeapon -= 1;
            }

        }
    }

}
