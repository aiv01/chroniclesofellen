using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{
    
    public class GameMgr : MonoBehaviour
    {

        private Area currentArea;

        private Transform lastSavePoint;

        private Progression gameStatus;


        private void Awake()
        {
            //TODO: leggo da file e assegno tutti i valori
            
        }

        private void ChangeProgression(Progression newProgression)
        {
            int actualProg = (int)gameStatus;
            int newProg = (int)newProgression;
            if (actualProg + 1 == newProg)
            {
                gameStatus = newProgression;
            }
        }

    }
}
