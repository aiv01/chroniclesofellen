using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;

namespace TheChroniclesOfEllen 
{
    public class EnemyMgr : MonoBehaviour
    {
        public BaseEnemyController chomperPrefab;
        public BaseEnemyController fastChomperPrefab;
        public BaseEnemyController spitterPrefab;
        public Transform[] chomperSpawnPoints;
        public Transform[] fastChomperSpawnPoints;
        public Transform[] spitterSpawnPoints;

        //TODO: da vedere se usare le aree, la progressione o enrambe
        //TODO: capire come fare per i patrol point
        public void CreatePool(Progression progression)
        {
            int i;
            int level = progression == Progression.TempleEntr ? 1 : progression == Progression.Boss1Dead ? 2 : 3;
            BaseEnemyController go;
            for (i = 0; i < chomperSpawnPoints.Length; i++)
            {
                go = Instantiate<BaseEnemyController>(chomperPrefab, chomperSpawnPoints[i]);
                go.enemySO = Resources.Load<BaseEnemySO>("ChomperLiv" + level);
            }

            for (i = 0; i < chomperSpawnPoints.Length; i++)
            {
                go = Instantiate<BaseEnemyController>(fastChomperPrefab, fastChomperSpawnPoints[i]);
                go.enemySO = Resources.Load<BaseEnemySO>("FastChomperLiv" + level);
            }

            for (i = 0; i < chomperSpawnPoints.Length; i++)
            {
                go = Instantiate<BaseEnemyController>(spitterPrefab, spitterSpawnPoints[i]);
                go.enemySO = Resources.Load<BaseEnemySO>("SpitterLiv" + level);
            }
        }

    }
}
