using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public class ShieldPowerUp : PowerUps
    {
        public int resistence;
        private int hitLeft;

        public override void OnHit()
        {
            if (true)
            {
                //evita il danno
                hitLeft--;
            }
        }

        public override void OnStart()
        {
            ShieldPUSO pu = (ShieldPUSO)powerUpsSO;
            resistence = pu.resistence;
            hitLeft = resistence;
        }

        public override void OnUpdate()
        {
            if (hitLeft == 0)
            {
                //rimuovo il powerup
            }
        }

        public override void ResetPowerUp()
        {
            hitLeft = resistence;
        }
    }
}