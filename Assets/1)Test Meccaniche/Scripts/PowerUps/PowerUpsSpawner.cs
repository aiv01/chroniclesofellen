using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class PowerUpsSpawner : MonoBehaviour
    {
        private PowerUps[,] powerups;
        [SerializeField]
        private PowerUps prefabPowerUp;

        private int instancePerPoweUp;

        // Start is called before the first frame update
        void Start()
        {
            InitSpawner();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void InitSpawner()
        {
            string path = "powerUpSO/";
            powerups = new PowerUps[(int)PowerUpType.Last, instancePerPoweUp];

            for (int i = 0; i < powerups.GetLength(0); i++) 
            {
                switch (i)
                {
                    case 0:
                        path += "Shield";
                        break;
                    case 1:
                        path += "Gun";
                        break;
                    case 2:
                        path += "Health";
                        break;
                    case 3:
                        path += "Permanent";
                        break;
                }
                for (int y = 0; y < powerups.GetLength(1); y++) 
                {
                    PowerUps pu = Instantiate<PowerUps>(prefabPowerUp);
                    pu.gameObject.SetActive(false);
                    pu.powerUpsSO = Resources.Load<PowerUpSO>(path);
                    pu.powerUpsSO.SetPowerUpType();
                    pu.OnStart();

                    powerups[i, y] = pu;
                    //vedere come fare per il componente powerUp di base
                }
            }
        }

        public PowerUps SpawnPowerUp(PowerUpType type)
        {
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