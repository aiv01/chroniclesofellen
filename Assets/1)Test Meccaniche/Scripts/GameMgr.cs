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
        public SafeFileSO defaultFile;

        private void Start()
        {
            currentFile = new SafeFile();
            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LoadNew();
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

        public void FoundKey()
        {
            currentFile.HasKey = true;
        }
        public void AddDoubleJump()
        {
            currentFile.HasDoubleJump = true;
        }
        public void AddDash()
        {
            currentFile.HasDash = true;
        }

        public void IncrementHealth(float value)
        {
            currentFile.MaxHp += (int)value;
        }
        public void IncrementDamage(float value)
        {
            currentFile.DamageScale += (int)value;
        }

        public void Save(int savePoint = 0)
        {
            currentFile.SavePointNumber = savePoint;
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
        public void LoadNew()
        {
            currentFile.MaxHp = defaultFile.MaxHp;
            currentFile.DamageScale = defaultFile.DamageScale;
            currentFile.HasDash = defaultFile.HasDash;
            currentFile.HasDoubleJump = defaultFile.HasDoubleJump;
            currentFile.HasKey = defaultFile.HasKey;
            currentFile.Progression = defaultFile.Progression;
            currentFile.Area = defaultFile.Area;
            currentFile.SavePointNumber = defaultFile.SavePointNumber;
            Debug.Log("Load New: " + currentFile.ToString());
        }
    }
}
