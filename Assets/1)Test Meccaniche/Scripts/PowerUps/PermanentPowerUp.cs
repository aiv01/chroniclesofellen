using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{

    public class PermanentPowerUp : PowerUp
    {
        [SerializeField]
        private UnityEvent<float> IncrementHealth;
        [SerializeField]
        private UnityEvent<float> IncrementDamage;

        private PermanentPUSO permanentSO;

        public override void OnStart()
        {
            permanentSO = (PermanentPUSO)powerUpsSO;
        }

        public override void OnPickUp()
        {
            IncrementHealth.Invoke(permanentSO.healthIncrease);
            IncrementDamage.Invoke(permanentSO.damageIncrease);
        }
    }
}
