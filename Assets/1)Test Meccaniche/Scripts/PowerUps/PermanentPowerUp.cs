using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{

    public class PermanentPowerUp : PowerUps
    {
        [SerializeField]
        private UnityEvent<float> IncrementHealth;
        [SerializeField]
        private UnityEvent<float> IncrementDamage;

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
            IncrementHealth.Invoke(healthIncrease);
            IncrementDamage.Invoke(damageIncrease);
        }
    }
}
