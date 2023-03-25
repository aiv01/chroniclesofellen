using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class GunPowerUp : PowerUp
    {
        private GunPUSO gunSO;
        public int bulletLeft;

        public override void OnStart()
        {
            gunSO = (GunPUSO)powerUpsSO;
            bulletLeft = gunSO.chargeBulletNumber;
        }

        public override bool OnShoot()
        {
            bulletLeft--;
            if (bulletLeft <= 0)
            {
                return false;
            }
            return true;
        }

        public override void ResetPowerUp()
        {
            bulletLeft = gunSO.chargeBulletNumber;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Palyer")
            {
                gameObject.SetActive(false);
                other.GetComponent<PlayerPowerUp>().ChangePowerUp(this);
                OnPickUp();
            }
        }
    }
}
