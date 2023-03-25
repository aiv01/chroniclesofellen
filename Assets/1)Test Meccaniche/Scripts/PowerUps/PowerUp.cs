using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public abstract class PowerUp : MonoBehaviour
    {
        public PowerUpSO powerUpsSO;

        public PowerUpType type;

        protected bool hasDoneHisJob;

        public bool HasDoneHisJob
        {
            get { return hasDoneHisJob; }
        }


        public virtual void OnStart() 
        {
            powerUpsSO.SetPowerUpType();
        }
        public virtual bool OnHit() { return false; }
        public virtual void OnUpdate() { }
        public virtual void ResetPowerUp() { }
        public virtual void OnPickUp() 
        {
            ResetPowerUp();
        }
        public virtual bool OnShoot() { return false; }

    }
}
