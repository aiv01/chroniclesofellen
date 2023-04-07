using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class PowerUpsSpawner : MonoBehaviour
    {
        private static PowerUp[,] powerups;
        [SerializeField]
        private PowerUp[] prefabPowerUp;

        private int instancePerPoweUp;
        private void Awake()
        {
            instancePerPoweUp = 15;
            InitSpawner();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitSpawner()
        {
            powerups = new PowerUp[(int)PowerUpType.None, instancePerPoweUp];

            for (int i = 0; i < powerups.GetLength(0); i++) 
            {
                for (int y = 0; y < powerups.GetLength(1); y++) 
                {
                    PowerUp pu = Instantiate<PowerUp>(prefabPowerUp[i]);
                    pu.gameObject.SetActive(false);
                    pu.transform.SetParent(transform, false);
                    pu.OnStart();
                    powerups[i, y] = pu;
                }
            }
        }

        public static PowerUp SpawnPowerUp(PowerUpType type)
        {
            if(type== PowerUpType.None) 
                return null;

            for(int i = 0; i < powerups.GetLength(1); i++)
            {
                if (!powerups[(int)type, i].gameObject.activeInHierarchy)
                {
                    return powerups[(int)type, i];
                }
            }
            return null;
        }
    }

}