using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class ChomperAudio : MonoBehaviour
    {
        private AudioSource source;
        [SerializeField]
        private AudioClip[] walkClips;
        [SerializeField]
        private AudioClip[] runClips;
        [SerializeField]
        private AudioClip[] attackClips;
        [SerializeField]
        private AudioClip[] damageClips;

        void Awake()
        {
            source = GetComponent<AudioSource>();

        }

        private void PlayWalkMovementAudio()
        {
            int random = Random.Range(0, walkClips.Length);
            source.PlayOneShot(walkClips[random]);
        }

        private void PlayRunMovementAudio()
        {
            int random = Random.Range(0,runClips.Length);
            source.PlayOneShot(runClips[random]);
        }

        private void PlayAttackAudio()
        {
            int random = Random.Range(0,attackClips.Length);
            source.PlayOneShot(attackClips[random]);
        }

        public void PlayDamageAudio()
        {
            int random = Random.Range(0,damageClips.Length);
            source.PlayOneShot(damageClips[random]);
        }

    }
}
