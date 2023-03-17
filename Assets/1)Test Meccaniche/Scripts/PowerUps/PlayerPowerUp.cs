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
        private PowerUps power;
        public PowerUpType CurrentPUType
        {
            get { return HasPowerUp ? power.type : PowerUpType.Last; }
        }
        public bool HasPowerUp
        {
            get { return power != null; }
        }


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangePowerUp(PowerUps newPowerUp)
        {
            if (newPowerUp.type == CurrentPUType)
            {
                power.ResetPowerUp();
                return;
            }
            power = newPowerUp;
        }


    }

}