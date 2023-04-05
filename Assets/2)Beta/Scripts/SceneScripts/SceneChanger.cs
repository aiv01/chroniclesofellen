using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace TheChroniclesOfEllen
{

    public class SceneChanger : MonoBehaviour
    {
        [SerializeField]
        private Area nextArea;
        [SerializeField]
        private int nextTeleportId;
        [SerializeField]
        private SceneLoader currSceneLoader;

        private void OnTriggerEnter(Collider other)
        {
            
            var v = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            if(nextArea == Area.Ship && !v.HasKey)
            {
                return;
            }
            v.SavePointNumber = nextTeleportId;
            v.Area = nextArea;
            if (nextArea == Area.Ship && v.HasKey)
            {
                Debug.Log("Vittoria");
                currSceneLoader.LoadVictory();
                return;
            }
            var t = JsonUtility.ToJson(v);
            File.WriteAllText(Application.persistentDataPath + "/JsonFile/DataFile.json", t);
            currSceneLoader.LoadScene(nextArea);
        }
    }

}