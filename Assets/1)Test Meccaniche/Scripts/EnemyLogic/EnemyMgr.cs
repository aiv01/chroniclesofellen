using System.Collections;
using System.Collections.Generic;
using TheChroniclesOfEllen;
using UnityEngine;

namespace TheChroniclesOfEllen 
{
    public class EnemyMgr : MonoBehaviour
    {
        public ChomperController chomperPrefab;
        public ChomperController fastChomperPrefab;
        public SpitterController spitterPrefab;
        public Transform[] chomperSpawnPoints;
        public Transform[] fastChomperSpawnPoints;
        public Transform[] spitterSpawnPoints;

        //TODO: da vedere se usare le aree, la progressione o enrambe
        //TODO: capire come fare per i patrol point
        public void CreatePool(Progression progression)
        {
            int i;
            int level = progression == Progression.TempleEntr ? 1 : progression == Progression.Boss1Dead ? 2 : 3;
            for (i = 0; i < chomperSpawnPoints.Length; i++)
            {
                ChomperController go = Instantiate<ChomperController>(chomperPrefab, chomperSpawnPoints[i]);
                go.enemySO = Resources.Load<BaseEnemySO>("ChomperLiv" + level);
            }

            for (i = 0; i < chomperSpawnPoints.Length; i++)
            {
                ChomperController go = Instantiate<ChomperController>(fastChomperPrefab, fastChomperSpawnPoints[i]);
                go.enemySO = Resources.Load<BaseEnemySO>("FastChomperLiv" + level);
            }

            for (i = 0; i < chomperSpawnPoints.Length; i++)
            {
                SpitterController go = Instantiate<SpitterController>(spitterPrefab, spitterSpawnPoints[i]);
                go.enemySO = Resources.Load<BaseEnemySO>("SpitterLiv" + level);
            }
        }

    }
}
