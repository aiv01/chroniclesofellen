using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class ShieldPowerUp : PowerUp
    {
        [SerializeField]
        private UnityEvent<bool> OnShieldDestroy;
        private ShieldPUSO shieldSO;
        private int hitLeft;

        public override void OnHit()
        {
            hitLeft--;
            if (hitLeft == 0) 
            {
                OnShieldDestroy.Invoke(false);
            }
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