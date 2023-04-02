using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class AudioMgr : MonoBehaviour
    {
       
       public Sound[] sounds;
       [Range(0f,10f)]
       private float generalVolume;
       [Range(0f,10f)]
       private float generalPitch;

       void Awake()
       {

            DontDestroyOnLoad(gameObject);

            foreach(var sound in sounds)
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
           Play("Gameplay");
       }

       private void Play(string name)
       {
            foreach(var s in sounds)
            {
                if(s == null) return;
                
                if(s.name == name)
                {
                    s.source.Play();
                }
            }
       }

    }
}
