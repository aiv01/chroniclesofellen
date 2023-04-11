using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheChroniclesOfEllen
{
    public class UIInputMgr : MonoBehaviour
    {

        private PlayerInput UIInput;
        public static UIInputMgr instance;

        void Awake()
        {

            UIInput = new PlayerInput();
            
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            UIInput.UI.Navigate.started += OnUINagivate;
            UIInput.UI.Navigate.performed += OnUINagivate;
            UIInput.UI.Navigate.canceled += OnUINagivate;
            UIInput.UI.Point.started += OnUINagivate;
            UIInput.UI.Point.performed += OnUINagivate;
            UIInput.UI.Point.canceled += OnUINagivate;
        }

        void OnUINagivate(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AudioMgr.instance.PlayOneShot("Navigate UI");
            }
        }
    }
}
