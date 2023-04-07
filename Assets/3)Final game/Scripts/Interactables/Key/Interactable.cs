using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public abstract class Interactable : MonoBehaviour
    {

        [SerializeField]
        protected UnityEvent<bool> ItemFound;
        protected virtual void OnPickUp(PlayerController player) { }

    }
}
