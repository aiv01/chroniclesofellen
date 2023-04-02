using Cinemachine.Utility;
using System.Collections;
using System.Collections;

namespace TheChroniclesOfEllen
{

    public class PermanentPowerUp : PowerUp
    {

        private PermanentPUSO permanentSO;

        public override void OnStart()
        {
            permanentSO = (PermanentPUSO)powerUpsSO;
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (other.tag == "Palyer")
            {
                gameObject.SetActive(false);
                other.GetComponent<HealthComponent>().IncreaseMaxHealth((int)permanentSO.healthIncrease);
                //TODO: incrementare il danno
                OnPickUp();
            }
        }
    }
}
