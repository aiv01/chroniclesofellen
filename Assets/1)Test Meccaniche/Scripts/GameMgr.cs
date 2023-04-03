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
        [SerializeField]
        private Area currentArea;
        public SceneLoader currSceneLoader;

        public BaseBossController areaBoss;

        private Vector3 lastTeleport;
        [SerializeField]
        private PlayerController player;

        public TextAsset textAsset;
        public SafeFile currentFile;
        public SafeFileSO defaultFile;
        public int currSavepointNumber;

        private void Awake()
        {
            Debug.Log(Application.persistentDataPath);
            currentFile = new SafeFile();
            LoadMenu();
        }

        private void Update()
        {

        }

        
        public void ChangeGolemStatus(BossStatus newProgression)
        {
            currentFile.GolemStatus = newProgression;
        }
        public void ChangeSpitterStatus(BossStatus newProgression)
        {
            currentFile.MotherSpitterStatus = newProgression;
        }
        public void ChangeArea(Area newArea)
        {
            currentArea = newArea;
        }

        public void FoundKey(bool value)
        {
            currentFile.HasKey = value;
            keyUI.enabled = value;
            ChangeGolemStatus(BossStatus.Active);
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
            currentFile.Area = currentArea;
            string saveData = JsonUtility.ToJson(currentFile);
            File.WriteAllText(Application.persistentDataPath + "/JsonFile/DataFile.json", saveData);
            Debug.Log("Save: " + currentFile.ToString());
        }
        public void LoadMenu()
        {
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            Debug.Log("Load: " + currentFile.ToString());
            Debug.Log(currentFile.SavePointNumber);
            lastTeleport = currSceneLoader.GetTeleportPosition(currentFile.SavePointNumber);
            player.transform.position = lastTeleport;
            keyUI.enabled=currentFile.HasKey;
            player.playerHealth.SetMaxHealth(currentFile.MaxHp); 

            if (currentFile.HasKey)
            {
                currSceneLoader.ChangeEnemyLevel(3);
            }
        }
        public void LoadSavePoint()
        {
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            currSceneLoader.LoadScene((Area)currentFile.Area);

            if (currentFile.HasKey)
            {
                currSceneLoader.ChangeEnemyLevel(2);
            }
        }
        public void LoadNew()
        {
            currentFile.MaxHp = defaultFile.MaxHp;
            currentFile.DamageScale = defaultFile.DamageScale;
            currentFile.HasDash = defaultFile.HasDash;
            currentFile.HasDoubleJump = defaultFile.HasDoubleJump;
            currentFile.HasKey = defaultFile.HasKey;
            currentFile.GolemStatus = defaultFile.GolemStatus;
            currentFile.MotherSpitterStatus = defaultFile.MotherSpitterStatus;
            currentFile.Area = defaultFile.Area;
            currentFile.SavePointNumber = defaultFile.SavePointNumber;
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            currSceneLoader.LoadNew();
        }
        
    }
}
