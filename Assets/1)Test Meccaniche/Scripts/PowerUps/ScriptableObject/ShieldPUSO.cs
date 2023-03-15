using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    [CreateAssetMenu(menuName = "Window/Shield", fileName = "Shield")]
    public class ShieldPUSO : PowerUpSO
    {
        public int resistence;

        public override void SetPowerUpType()
        {
            type = PowerUpType.Shield;
        }
    }

}