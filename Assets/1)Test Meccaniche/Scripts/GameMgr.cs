using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.ShaderGraph.Serialization;

namespace TheChroniclesOfEllen
{

    public class GameMgr : MonoBehaviour
    {
        private Progression gameStatus;
        private Area currentArea;

        private Transform lastTeleport;

        public TextAsset text;
        public SafeFile currentFile;

        private void Start()
        {
            currentFile = new SafeFile();
            Load();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
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

        private void ChangeArea(Area newArea)
        {
            //fare dei controlli
            currentArea = newArea;
        }

        private void FoundKey()
        {
            currentFile.HasKey = true;
        }
        private void AddDoubleJump()
        {
            currentFile.HasDoubleJump = true;
        }

        public void Save()
        {
            

            string saveData = JsonUtility.ToJson(currentFile);
            text=new TextAsset(saveData);
            File.WriteAllText(Application.dataPath + "/1)Test Meccaniche/Resources/JsonFile/DataFile.json", saveData);
            Debug.Log("Save: " + currentFile.ToString());
        }

        public void Load()
        {
            currentFile = JsonUtility.FromJson<SafeFile>(text.text);
            Debug.Log("Load: " + currentFile.ToString());
        }
    }
}
