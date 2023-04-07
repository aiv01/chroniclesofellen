using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheChroniclesOfEllen
{

    public class HealthUI : MonoBehaviour
    {
        public Image heart;

        public void ChangeHeartStatus(bool value)
        {
            heart.gameObject.SetActive(value);
        }
    }
}