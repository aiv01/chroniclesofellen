using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;
using UnityEngine.UI;

namespace TheChroniclesOfEllen
{

    public class PlayerPowerUp : MonoBehaviour
    {
        //decidere se farlo con tot powerups contemporaneamente
        [SerializeField]
        private PowerUp power;
        public Image shieldUI;
        public Image gunUI;

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
            bool shieldStatus = false;
            if (power != null && CurrentPUType == PowerUpType.Shield)
            {
                shieldStatus = ((ShieldPowerUp)power).ShieldStatus;
                shieldUI.gameObject.SetActive(shieldStatus);
                return shieldStatus;
            }
            shieldUI.gameObject.SetActive(false);
            return false;
        }

        public bool HaveSpecialLeft()
        {

            bool specialActive = false;
            if (power != null && CurrentPUType == PowerUpType.Gun)
            {
                specialActive = power.OnShoot();
                gunUI.gameObject.SetActive(specialActive);
                if (!specialActive)
                {
                    ChangePowerUp(null);
                }
                return specialActive;
            }
            gunUI.gameObject.SetActive(false);
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