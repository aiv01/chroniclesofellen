using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class PlayerPowerUp : MonoBehaviour
    {
        //decidere se farlo con tot powerups contemporaneamente
        [SerializeField]
        private PowerUp power;

        public PowerUpType CurrentPUType
        {
            get { return power.type; }
        }

        // Update is called once per frame
        void Update()
        {
            if (power == null)
            {
                return;
            }
        }

        public void ChangePowerUp(PowerUp newPowerUp)
        {
            power = newPowerUp;

            if(power != null ) 
                power.OnStart();
        }

        public bool IsShieldActive()
        {
            if (power != null && CurrentPUType == PowerUpType.Shield)
            {
                bool shieldActive = power.OnHit();
                if (!shieldActive)
                {
                    ChangePowerUp(null);
                }
                return shieldActive;
            }
            return false;
        }

        public bool HaveSpecialLeft()
        {
            if (power != null && CurrentPUType == PowerUpType.Gun)
            {
                bool specialActive = power.OnShoot();
                if (!specialActive)
                {
                    ChangePowerUp(null);
                }
                return specialActive;
            }
            return false;
        }
    }
}