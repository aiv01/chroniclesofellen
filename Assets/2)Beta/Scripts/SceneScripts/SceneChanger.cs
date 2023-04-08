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
        [SerializeField]
        private GameMgr gameMgr;

        private void OnTriggerEnter(Collider other)
        {
            if (!Directory.Exists(Application.persistentDataPath + "/JsonFile"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/JsonFile");
            }
            var v = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            if(nextArea == Area.Ship && !v.HasKey)
            {
                return;
            }
            if (nextArea == Area.Ship && v.HasKey)
            {
                currSceneLoader.LoadVictory();
                return;
            }
            gameMgr.ChangeArea(nextArea);
            gameMgr.currSavepointNumber = nextTeleportId;
            gameMgr.Save();
            currSceneLoader.LoadScene(nextArea);
        }
    }

}