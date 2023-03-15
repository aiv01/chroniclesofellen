using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class PermanentPowerUp : PowerUps
    {
        public int damageIncrease;
        public int healthIncrease;

        public override void OnStart()
        {
            PermanentPUSO pu = (PermanentPUSO)powerUpsSO;
            damageIncrease = pu.damageIncrease;
            healthIncrease = pu.healthIncrease;
        }

        public override void OnPickUp()
        {
            //incremento le robe al giocatore
        }
    }
}
