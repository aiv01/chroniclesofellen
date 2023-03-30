using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    [System.Serializable]
    public class Sound : MonoBehaviour
    {
        public AudioClip clip;
        public float volume;
        public float pitch;
        [HideInInspector]
        public AudioSource source;
    }
}
