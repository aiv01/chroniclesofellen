using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class SavePoint : MonoBehaviour
    {
        [SerializeField]
        private int savePointNumber;
        [SerializeField]
        private Area savepointArea;
        [SerializeField]
        private GameMgr gameMgr;

        public Transform playerSpawn;
        public Transform saveUI;


        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                gameMgr.ChangeArea(savepointArea);
                gameMgr.currSavepointNumber = savePointNumber;
                saveUI.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                saveUI.gameObject.SetActive(false);
            }
        }
    }
}
