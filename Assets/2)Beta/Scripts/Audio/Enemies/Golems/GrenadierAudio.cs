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
        private AudioClip[] closeRangeAttackClips;
        [SerializeField]
        private AudioClip[] meleeAttackClips;
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
        private void PlayCloseRangeAttackAudio()
        {
            int random = Random.Range(0,closeRangeAttackClips.Length);
            source.PlayOneShot(closeRangeAttackClips[random]);
        }
        private void PlayMeleeAttackAudio()
        {
            int random = Random.Range(0,meleeAttackClips.Length);
            source.PlayOneShot(meleeAttackClips[random]);
        }
        private void PlayDeathAudio()
        {
            int random = Random.Range(0,deathClips.Length);
            source.PlayOneShot(deathClips[random]);
        }
    }
}