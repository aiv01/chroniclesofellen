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

            if (power != null)
            {
                power.ResetPowerUp();
            }
            

        }

        public bool IsShieldActive()
        {
            if (power != null && CurrentPUType == PowerUpType.Shield)
            {
                return ((ShieldPowerUp)power).ShieldStatus;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns if the hit is blocked</returns>
        public bool OnHit ()
        {
            if (power != null)
            {
                return power.OnHit();
            }
            return false;
        }
    }
}