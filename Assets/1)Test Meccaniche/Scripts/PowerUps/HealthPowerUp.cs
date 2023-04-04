using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    public class HealthPowerUp : PowerUp
    {
        [SerializeField]
        private HealthPUSO HealthSO;

        public override void OnStart()
        {
            HealthSO = (HealthPUSO)powerUpsSO;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                gameObject.SetActive(false);
                other.GetComponent<HealthComponent>().HealMe((int)HealthSO.healAmount);
                OnPickUp();
            }
        }
    }
}
