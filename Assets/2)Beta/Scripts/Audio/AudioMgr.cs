using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class AudioMgr : MonoBehaviour
    {
       [SerializeField]
       private Sound[] sounds;
       [SerializeField]
       [Range(0f,10f)]
       private float volume;
       [SerializeField]
       [Range(0f,10f)]
       private float pitch;

       void Awake()
       {
            foreach(var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
            }
       }
    }
}
