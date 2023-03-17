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
            power.OnHit();
        }

        public void ChangePowerUp(PowerUp newPowerUp)
        {
            power = newPowerUp;
        }
    }
}