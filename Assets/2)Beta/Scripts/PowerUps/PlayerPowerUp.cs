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
                if (power.type == PowerUpType.Shield)
                {
                    shieldUI.gameObject.SetActive(true);
                    gunUI.gameObject.SetActive(false);
                }
                if (power.type == PowerUpType.Gun)
                {
                    gunUI.gameObject.SetActive(true); 
                    shieldUI.gameObject.SetActive(false);
                }
                power.ResetPowerUp();
            }
            

        }

        public bool IsShieldActive()
        {
            
            if (power != null && CurrentPUType == PowerUpType.Shield)
            {
                bool shieldStatus = ((ShieldPowerUp)power).ShieldStatus;
                shieldUI.gameObject.SetActive(shieldStatus);
                return shieldStatus;
            }
            shieldUI.gameObject.SetActive(false);
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
                    gunUI.gameObject.SetActive(false);
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