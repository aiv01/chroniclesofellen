using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheChroniclesOfEllen
{

    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField]
        private Image bossHealth;

        public void ChangeBossHealth(float healthPerc)
        {
            bossHealth.fillAmount = healthPerc;
        }
    }

}