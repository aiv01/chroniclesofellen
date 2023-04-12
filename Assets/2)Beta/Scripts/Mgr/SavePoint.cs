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
        private bool isExitPause = true;

        public int SavePointNumber
        {
            get { return savePointNumber; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isExitPause = false;
                gameMgr.ChangeArea(savepointArea);
                gameMgr.currSavepointNumber = savePointNumber;
                saveUI.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                if (!isExitPause)
                {
                    Time.timeScale = 0;
                    other.gameObject.GetComponent<InputMgr>().enabled = false;
                    isExitPause = true;
                   
                }
                
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                saveUI.gameObject.SetActive(false);

                Time.timeScale = 1f;
                saveUI.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public void ExitMenu()
        {
            saveUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
