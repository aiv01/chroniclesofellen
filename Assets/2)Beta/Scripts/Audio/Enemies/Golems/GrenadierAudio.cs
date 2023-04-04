using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class GrenadierAudio : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField]
        private AudioClip[] movementClips;
        [SerializeField]
        private AudioClip[] attackClips;
        [SerializeField]
        private AudioClip[] deathClips;

        void Awake()
        {
            source  = GetComponent<AudioSource>();
        }

        private void PlayMovementAudio()
        {
            int random = Random.Range(0,movementClips.Length);
            source.PlayOneShot(movementClips[random]);
        }
        private void PlayDeathAudio()
        {
            int random = Random.Range(0,deathClips.Length);
            source.PlayOneShot(deathClips[random]);
        }
    }
}