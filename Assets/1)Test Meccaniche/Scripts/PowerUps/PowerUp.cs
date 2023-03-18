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

        public PowerUpType type;

        protected bool hasDoneHisJob;

        public bool HasDoneHisJob
        {
            get { return hasDoneHisJob; }
        }


        public virtual void OnStart() { }
        public virtual bool OnHit() { return false; }
        public virtual void OnUpdate() { }
        public virtual void ResetPowerUp() { }
        public virtual void OnPickUp() 
        {
            ResetPowerUp();
        }
        public virtual bool OnShoot() { return false; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                gameObject.SetActive(false);
                hasDoneHisJob = false;
                pickUp.Invoke(this);
                OnPickUp();
            }
        }

    }
}
