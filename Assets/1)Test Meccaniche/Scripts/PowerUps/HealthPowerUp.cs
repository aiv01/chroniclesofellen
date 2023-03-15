using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class HealthPowerUp : PowerUps
    {
        public int healAmount;

        public override void OnStart()
        {
            HealthPUSO pu = (HealthPUSO)powerUpsSO;
            healAmount = pu.healAmount;
        }
        public override void OnPickUp()
        {
            //curo il giocatore
        }
    }
}
