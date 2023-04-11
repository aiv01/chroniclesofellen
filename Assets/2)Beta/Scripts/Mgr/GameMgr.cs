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
            currentFile = new SafeFile();
            LoadMenu();
            
           
             
        }

        private void Start()
        {
           switch(currentArea)
            {
                
                case Area.Ship:
                AudioMgr.instance.Stop("Menu");
                AudioMgr.instance.Play("Level0");
                break;
                case Area.Temple1:
                AudioMgr.instance.Play("Level1");
                AudioMgr.instance.Stop("Level0");
                break;
                case Area.MainMenu:
                AudioMgr.instance.Play("Menu");
                AudioMgr.instance.Stop("Level0");
                AudioMgr.instance.Stop("Level1");
                break;
                case Area.GameOver:
                AudioMgr.instance.Stop("Level0");
                AudioMgr.instance.Stop("Level1");
                AudioMgr.instance.Stop("Boss Music");
                break;
                case Area.Victory:
                AudioMgr.instance.Stop("Level0");
                AudioMgr.instance.Stop("Level1");
                AudioMgr.instance.Stop("Boss Music");
                AudioMgr.instance.Play("Victory");
                break;
                


            }
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
            currentFile.HasKey = true;
            keyUI.gameObject.SetActive(true);
            ChangeGolemStatus(BossStatus.Active);
            currSceneLoader.ChangeEnemyLevel(3);
            Save();
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
            if (!Directory.Exists(Application.persistentDataPath + "/JsonFile"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/JsonFile");
            }
            currentFile.MaxHp = player.playerHealth.GetMaxHealth();
            currentFile.SavePointNumber = currSavepointNumber;
            currentFile.Area = currentArea;
            string saveData = JsonUtility.ToJson(currentFile);
            File.WriteAllText(Application.persistentDataPath + "/JsonFile/DataFile.json", saveData);
        }
        public void LoadMenu()
        {
            Time.timeScale = 1;
            if (!Directory.Exists(Application.persistentDataPath + "/JsonFile"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/JsonFile");
            }
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            lastTeleport = currSceneLoader.GetTeleportPosition(currentFile.SavePointNumber);
            player.transform.position = lastTeleport;
            player.GetComponent<CharacterController>().enabled = true;
            keyUI.gameObject.SetActive(currentFile.HasKey);
            player.playerHealth.SetMaxHealth(currentFile.MaxHp); 

            
            if (currentArea == Area.Temple2 && currentFile.MotherSpitterStatus == BossStatus.Active) 
            {
                areaBoss.gameObject.SetActive(true);
            }
            if(currentFile.HasKey)
            {
                
                if (currentArea == Area.Temple1)
                {
                    areaBoss.gameObject.SetActive(true);
                }
                currSceneLoader.ChangeEnemyLevel(3);
            }
        }
        public void LoadSavePoint()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/JsonFile"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/JsonFile");
            }
            currentFile = JsonUtility.FromJson<SafeFile>(File.ReadAllText(Application.persistentDataPath + "/JsonFile/DataFile.json"));
            if (currentFile == null)
            {
                LoadNew();
                return;
            }
            currSceneLoader.LoadScene((Area)currentFile.Area);
        }
        public void LoadNew()
        {
            if(!Directory.Exists(Application.persistentDataPath + "/JsonFile"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/JsonFile");
            }
            currentFile.MaxHp = defaultFile.MaxHp;
            currentFile.DamageScale = defaultFile.DamageScale;
            currentFile.HasDash = defaultFile.HasDash;
            currentFile.HasDoubleJump = defaultFile.HasDoubleJump;
            currentFile.HasKey = defaultFile.HasKey;
            currentFile.GolemStatus = defaultFile.GolemStatus;
            currentFile.MotherSpitterStatus = defaultFile.MotherSpitterStatus;
            currentFile.Area = defaultFile.Area;
            currentFile.SavePointNumber = defaultFile.SavePointNumber;
            string saveData = JsonUtility.ToJson(currentFile);
            File.WriteAllText(Application.persistentDataPath + "/JsonFile/DataFile.json", saveData);
            currSceneLoader.LoadNew();
        }
        
        public void Exit()
        {
            Application.Quit();
        }
        
        public void ExitUI()
        {
            Time.timeScale = 1;
            player.gameObject.GetComponent<InputMgr>().enabled = true;
            
        }
    }
}
