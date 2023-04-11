using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class Key : Interactable
    {
        void Update()
        {
            transform.Rotate(Vector3.up, Space.World);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                gameObject.SetActive(false);
                ItemFound.Invoke(true);
                Debug.Log("Chiave");
            }
        }

    }
}
