using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheChroniclesOfEllen
{

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SavePoint[] teleportPositions;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector3 GetTeleportPosition(int teleportID) 
        {
            return teleportID < teleportPositions.Length ? 
                teleportPositions[teleportID].transform.position : 
                teleportPositions[0].transform.position;
        }
    }

}