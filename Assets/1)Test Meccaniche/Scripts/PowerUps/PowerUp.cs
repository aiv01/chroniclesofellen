using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public abstract class PowerUp : MonoBehaviour
    {
        [SerializeField] 
        private UnityEvent<object> pickUp;

        public PowerUpSO powerUpsSO;

        private PowerUpType type;


        public virtual void OnStart() { }

        public virtual void OnUpdate() { }
        public virtual void OnHit() { }
        public virtual void ResetPowerUp() { }
        public virtual void OnPickUp() { }
        public virtual void OnShoot() { }

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
