using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    [CreateAssetMenu(menuName = "Window/Health", fileName = "Health")]
    public class HealthPUSO : PowerUpSO
    {
        public float healAmount;

        public override void SetPowerUpType()
        {
            type = PowerUpType.Health;
        }
    }

}