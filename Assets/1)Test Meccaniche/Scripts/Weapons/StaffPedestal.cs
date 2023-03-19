using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class StaffPedestal : Interactable
    {
        private float angle;
        private InteractableType type;
        public InteractableType Type { get { return type; } }
        private GameObject staffTransform;

        void Start()
        {
            staffTransform = transform.Find("Staff").gameObject;
            type = InteractableType.StaffPedestal;

        }
        private void Update()
        {
            angle += 4f * Time.deltaTime;
            staffTransform.gameObject.transform.localPosition = Vector3.forward * Mathf.Sin(angle) * 0.5f;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                ItemFound.Invoke(true);
                gameObject.SetActive(false);
            }
        }

        protected override void OnPickUp(PlayerController player)
        {
        }
    }
}
