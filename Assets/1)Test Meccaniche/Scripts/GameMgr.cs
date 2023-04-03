using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System;

namespace TheChroniclesOfEllen
{

    public class GameMgr : MonoBehaviour
    {
        public Image keyUI;
        public UIHealthBar playerHealthBar;
        private Progression gameStatus;
        private Area currentArea;
        public SceneLoader currSceneLoader;

        private Vector3 lastTeleport;
        [SerializeField]
        private PlayerController player;

        public TextAsset textAsset;
        public SafeFile currentFile;
        public SafeFileSO defaultFile;
        public int currSavepointNumber;



        private void Start()
        {
            currentFile = new SafeFile();
            LoadMenu();
        }

        private void Update()
        {
        }

        
        public void ChangeProgression(Progression newProgression)
        {
            int actualProg = (int)gameStatus;
            int newProg = (int)newProgression;
            if (actualProg + 1 == newProg)
            {
                gameStatus = newProgression;
            }
        }
        public void ChangeArea(Area newArea)
        {
            currentArea = newArea;
        }

        public void FoundKey(bool value)
        {
            currentFile.HasKey = value;
            keyUI.enabled = value;
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
            MathF.Min(currentFile.MaxHp += (int)value, 10);
            
        }
        public void IncrementDamage(float value)
        {
            currentFile.DamageScale += (int)value;
        }

        public void Save()
        {
            currentFile.MaxHp = player.playerHealth.GetMaxHealth();
            currentFile.SavePointNumber = currSavepointNumber;
            currentFile.Area = (int)currentArea;
            currentFile.Progression = (int)gameStatus;
            string saveData = JsonUtility.ToJson(currentFile);
            File.WriteAllText(Application.persistentDataPath + "/JsonFile/DataFile.json", saveData);
            Debug.Log("Save: " + currentFile.ToString());
        }
        public void LoadMenu()
        {
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            Debug.Log("Load: " + currentFile.ToString());
            lastTeleport = currSceneLoader.GetTeleportPosition(0);
            player.transform.position = lastTeleport;
            keyUI.enabled=currentFile.HasKey;
            player.playerHealth.SetMaxHealth(currentFile.MaxHp);
        }
        public void LoadSavePoint()
        {
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            currSceneLoader.LoadScene((Area)currentFile.Area);
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
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            currSceneLoader.LoadNew();
        }
        
    }
}
