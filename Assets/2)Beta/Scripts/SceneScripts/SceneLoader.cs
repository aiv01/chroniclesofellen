using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheChroniclesOfEllen
{

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SavePoint[] teleportPositions;
        [SerializeField]
        private Dictionary<Area, string> areas = new Dictionary<Area, string>
            {
                { Area.Ship, "Level0" },
                { Area.Temple1,"Level1" },
                { Area.Temple2, "Level2" }
            };

        [SerializeField]
        private ChomperController[] chompers;
        [SerializeField]
        private SpitterController[] spitters;

        public Vector3 GetTeleportPosition(int teleportID)
        {
            foreach(SavePoint s in teleportPositions)
            {
                if (s.SavePointNumber == teleportID)
                {
                    return s.playerSpawn.position;
                }
            }
            return teleportPositions[0].playerSpawn.position;
        }
        public void LoadScene(Area newArea)
        {
            SceneManager.LoadScene(areas[newArea], LoadSceneMode.Single);
        }
        public void LoadNew()
        {
            SceneManager.LoadScene(areas[Area.Ship], LoadSceneMode.Single);
        }
        public void LoadMenu()
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
        public void LoadVictory()
        {
            SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
        }
        public void LoadGameOver()
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }

        public void ChangeEnemyLevel(int levelNumber)
        {
            BaseEnemySO chomperSO = Resources.Load<BaseEnemySO>("enemySO/ChomperLiv" + levelNumber);
            for (int i = 0; i < chompers.Length; i++)
            {
                chompers[i].enemySO = chomperSO;
                chompers[i].ReloadChomper();
                chompers[i].gameObject.SetActive(true);
            }
            BaseEnemySO spitterSO = Resources.Load<BaseEnemySO>("enemySO/SpitterLiv" + levelNumber);

            for (int i = 0; i < spitters.Length; i++)
            {
                spitters[i].enemySO = spitterSO;
                spitters[i].ReloadSpitter();
                spitters[i].gameObject.SetActive(true);
            }
        }
    }

}