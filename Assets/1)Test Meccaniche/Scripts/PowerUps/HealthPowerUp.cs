using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{
    public class HealthPowerUp : PowerUps
    {
        [SerializeField]
        private UnityEvent<int> Heal;
        public int healAmount;

        public override void OnStart()
        {
            HealthPUSO pu = (HealthPUSO)powerUpsSO;
            healAmount = pu.healAmount;
        }
        public override void OnPickUp()
        {
            Debug.Log("Ociuccio è curato di " + healAmount);
            Heal.Invoke(healAmount);
        }
    }
}
