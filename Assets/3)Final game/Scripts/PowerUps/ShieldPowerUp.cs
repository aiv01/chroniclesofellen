using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public class ShieldPowerUp : PowerUp
    {
        [SerializeField]
        private ShieldPUSO shieldSO;
        private int hitLeft;
        public bool ShieldStatus
        {
            get { return (hitLeft > 0); }
        }

        public override bool OnHit()
        {
            if (hitLeft == 0) 
            {
                return false;
            }
            hitLeft--;
            return true;
        }

        public override void OnStart()
        {
            shieldSO = (ShieldPUSO)powerUpsSO;

            hitLeft = shieldSO.resistence;
        }

        public override void OnUpdate()
        {
        }

        public override void ResetPowerUp()
        {
            hitLeft = shieldSO.resistence;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                gameObject.SetActive(false);
                other.GetComponent<PlayerPowerUp>().ChangePowerUp(this);
                OnPickUp();
            }
        }
    }
}