using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class HealthPowerUp : PowerUp
    {
        [SerializeField]
        private UnityEvent<float> Heal;
        private HealthPUSO HealthSO;

        public override void OnStart()
        {
            HealthSO = (HealthPUSO)powerUpsSO;
        }
        public override void OnPickUp()
        {
            Heal.Invoke(HealthSO.healAmount);
        }
    }
}
