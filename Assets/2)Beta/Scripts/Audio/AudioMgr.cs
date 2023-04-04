using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace TheChroniclesOfEllen
{
    public class AudioMgr : MonoBehaviour
    {

        public Sound[] sounds;
        [Range(0f, 10f)]
        private float generalVolume;
        [Range(0f, 10f)]
        private float generalPitch;
        public static AudioMgr instance;
        private Scene actualScene;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        void Start()
        {
            actualScene = SceneManager.GetActiveScene();
            switch (actualScene.name)
            {
                case "Level0":
                    Play("Level0");
                    break;
                case "Level1":
                    Play("Level1");
                    break;
                case "Level2":
                    Play("Battle");
                    break;

            }
        }
        void Update()
        {
            actualScene = SceneManager.GetActiveScene();
            switch (actualScene.name)
            {
                case "Level0":
                    Play("Level0");
                    break;
                case "Level1":
                    Play("Level1");
                    break;
                case "Level2":
                    Play("Battle");
                    break;

            }
        }

        private void Play(string name)
        {
            foreach (var s in sounds)
            {
                if (s == null) return;

                if (s.name == name)
                {
                    s.source.Play();
                }
            }
        }

    }
}
