using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class BossArenaTrigger : MonoBehaviour
    {
        [SerializeField]
        private BaseBossController boss;
        [SerializeField]
        private Transform[] arenaWalls;
        public bool isBossActive;

        private void OnTriggerEnter(Collider other)
        {
            if (boss.gameObject.activeInHierarchy)
            {
                boss.enabled = true;
                for (int i = 0; i < arenaWalls.Length; i++)
                {
                    arenaWalls[i].gameObject.SetActive(true);
                }
            }
        }
    }

}