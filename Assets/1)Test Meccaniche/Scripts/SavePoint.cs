using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheChroniclesOfEllen
{

    public class SavePoint : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<int> SaveTheGame;
        [SerializeField]
        private UnityEvent LoadTheGame;
        [SerializeField]
        private int savePointNumber;

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                if (Input.GetKey(KeyCode.L))
                {
                    LoadTheGame.Invoke();
                }
                if (Input.GetKey(KeyCode.K))
                {
                    SaveTheGame.Invoke(savePointNumber);
                }
            }
        }
    }
}
