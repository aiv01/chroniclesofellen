using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [SerializeField]
        [Range(0f,10f)]
        public float volume;
        [SerializeField]
        [Range(0f,10f)]
        public float pitch;
        [HideInInspector]
        public AudioSource source;
        public bool loop;
    }
}
