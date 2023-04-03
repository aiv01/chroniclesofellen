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

        public Vector3 GetTeleportPosition(int teleportID) 
        {
            return teleportID < teleportPositions.Length ? 
                teleportPositions[teleportID].transform.position : 
                teleportPositions[0].transform.position;
        }

        public void LoadScene(Area newArea)
        {
            SceneManager.LoadScene(areas[newArea], LoadSceneMode.Single);
        }

        public void LoadNew()
        {
            SceneManager.LoadScene(areas[Area.Ship], LoadSceneMode.Single);
        }
    }

}