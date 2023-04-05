using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    [CreateAssetMenu(menuName = "Window/Gun", fileName = "Gun")]
    public class GunPUSO : PowerUpSO
    {
        public int chargeBulletNumber;
        public float damageScale;

        public override void SetPowerUpType()
        {
            type = PowerUpType.Gun;
        }
    }

}