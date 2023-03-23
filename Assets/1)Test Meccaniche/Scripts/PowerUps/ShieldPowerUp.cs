using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class ShieldPowerUp : PowerUp
    {
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
    }
}