using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace TheChroniclesOfEllen
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] walkMovementAudioClips;
        [SerializeField]
        private AudioClip[] runMovementAudioClips;
        [SerializeField]
        private AudioClip[] deathAudioClips;
        [SerializeField]
        private AudioClip[] jumpAudioClips;
        [SerializeField]
        private AudioClip[] meleeAttackAudioClips;
        private AudioSource source;

        void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void PlayWalkMovementAudio()
        {
            int random = Random.Range(0,walkMovementAudioClips.Length);
            source.PlayOneShot(walkMovementAudioClips[random]);
        }
        private void PlayRunMovementAudio()
        {
            int random = Random.Range(0, runMovementAudioClips.Length);
            source.PlayOneShot(runMovementAudioClips[random]);

        }
        public void PlayDeathAudio()
        {
            int random = Random.Range(0,deathAudioClips.Length);
            source.PlayOneShot(deathAudioClips[random]);
        }
        public void PlayJumpAudio()
        {
            int random = Random.Range(0,jumpAudioClips.Length);
            if(gameObject.GetComponent<InputMgr>().IsJumpPressed)
            {
                 source.PlayOneShot(jumpAudioClips[random]);
            }
           
            
        }
    }
}
