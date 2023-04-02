
using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class GunAudio : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField]
        private AudioClip[] bulletShootClips;

        void Awake()
        {
            source = GetComponent<AudioSource>();

        }

        public void PlayShootAudio()
        {
            int random = Random.Range(0, bulletShootClips.Length);
            source.PlayOneShot(bulletShootClips[random]);
        }
    }
}
