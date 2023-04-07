using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class SpitterAudio : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField]
        private AudioClip[] runClips;
        [SerializeField]
        private AudioClip[] attackAudioClips;
        [SerializeField]
        private AudioClip[] damageAudioClips;

        void Awake()
        {
            source = GetComponent<AudioSource>();
        }
        private void PlayRunMovementAudio()
        {
            int random = Random.Range(0,runClips.Length);
            source.PlayOneShot(runClips[random]);
        }
        private void Shoot()
        {
            int random = Random.Range(0,attackAudioClips.Length);
            source.PlayOneShot(attackAudioClips[random]);
        }
         private void PlayDamageAudio()
        {
            int random = Random.Range(0,damageAudioClips.Length);
            source.PlayOneShot(damageAudioClips[random]);
        }
        
    }
}