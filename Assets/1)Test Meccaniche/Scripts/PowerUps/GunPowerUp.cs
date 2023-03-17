using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{

    public class GunPowerUp : PowerUp
    {
        [SerializeField]
        private UnityEvent BulletEnd;
        private GunPUSO gunSO;
        private int bulletLeft;

        public override void OnHit()
        {
        }

        public override void OnStart()
        {
            gunSO = (GunPUSO)powerUpsSO;
            bulletLeft = gunSO.chargeBulletNumber;
        }

        public override void OnUpdate()
        {
        }

        public override void OnShoot()
        {
            bulletLeft--;
            if (bulletLeft == 0)
            {
                BulletEnd.Invoke();
            }
        }

        public override void ResetPowerUp()
        {
            bulletLeft = gunSO.chargeBulletNumber;
        }
    }
}
