using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public abstract class PowerUps : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent<object> pickUp;

        public PowerUpSO powerUpsSO;

        public PowerUpType type;


        public virtual void OnStart() { }

        public virtual void OnUpdate() { }
        public virtual void OnHit() { }
        public virtual void ResetPowerUp() { }
        public virtual void OnPickUp() { }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                gameObject.SetActive(false);
                pickUp.Invoke(this);
                OnPickUp();
            }
        }

    }
}
