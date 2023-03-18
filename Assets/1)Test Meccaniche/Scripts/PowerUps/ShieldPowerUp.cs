using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class ShieldPowerUp : PowerUp
    {
        private ShieldPUSO shieldSO;
        public int hitLeft;

        public override bool OnHit()
        {
            hitLeft--;
            if (hitLeft == 0) 
            {
                return true;
            }
            return false;
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
    }
}