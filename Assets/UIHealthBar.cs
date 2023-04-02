using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheChroniclesOfEllen
{

    public class UIHealthBar : MonoBehaviour
    {
        public HealthUI[] healthBar;
        public int currPlayerMaxHealth;
        public int currPlayerHealth;

        public void LoadHealth(int maxHealth)
        {
            maxHealth = Mathf.Clamp(maxHealth, 0, 10);
            currPlayerMaxHealth = maxHealth;
            for(int i = 0; i < maxHealth; i++)
            {
                healthBar[i].gameObject.SetActive(true);
            }
            for(int i = maxHealth; i < healthBar.Length; i++)
            {
                healthBar[i].gameObject.SetActive(false);
            }
        }
        public void ChangeHealth(int currHealth)
        {
            currHealth= Mathf.Clamp(currHealth, 0, currPlayerMaxHealth);
            currPlayerHealth = currHealth;
            for (int i = 0; i < currHealth; i++)
            {
                healthBar[i].ChangeHeartStatus(true);
            }
            for (int i = currHealth; i < currPlayerMaxHealth; i++)
            {
                healthBar[i].ChangeHeartStatus(false);
            }
        }
    }
}
