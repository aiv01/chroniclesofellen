using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public class HealthPowerUp : PowerUp
    {
        private HealthPUSO HealthSO;

        public override void OnStart()
        {
            HealthSO = (HealthPUSO)powerUpsSO;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Palyer")
            {
                gameObject.SetActive(false);
                other.GetComponent<HealthComponent>().HealMe(HealthSO.healAmount);
                OnPickUp();
            }
        }
    }
}
