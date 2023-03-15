using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class GunPowerUp : PowerUps
    {
        public int maxBullet;
        private int currentBullet;
        public float damageScale;

        public override void OnHit()
        {
        }

        public override void OnStart()
        {
            GunPUSO pu = (GunPUSO)powerUpsSO;
            maxBullet = pu.chargeBulletNumber;
            currentBullet = maxBullet;
            damageScale = pu.damageScale;
        }

        public override void OnUpdate()
        {
            if (currentBullet == 0)
            {
                //rimuovo il powerup
            }
            //condizione per far sparare il colpo caricato
            if (true)
            {
                currentBullet--;
            }
        }

        public override void ResetPowerUp()
        {
            currentBullet = maxBullet;
        }
    }
}
