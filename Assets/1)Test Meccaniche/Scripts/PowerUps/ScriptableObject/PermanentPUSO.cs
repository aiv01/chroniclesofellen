using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    [CreateAssetMenu(menuName = "Window/Permanent", fileName = "Permanent")]
    public class PermanentPUSO : PowerUpSO
    {
        public int damageIncrease;
        public int healthIncrease;

        public override void SetPowerUpType()
        {
            type = PowerUpType.Permanent;
        }
    }

}