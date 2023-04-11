using System.Collections;
using UnityEngine;
using UnityEngine.Audio;


namespace TheChroniclesOfEllen
{
    public class AudioMgr : MonoBehaviour
    {

        public Sound[] sounds;
        [Range(0f, 10f)]
        private float generalVolume;
        [Range(0f, 10f)]
        private float generalPitch;
        public static AudioMgr instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        public void Play(string name)
        {
            foreach (var s in sounds)
            {
                if (s == null) return;

                if (s.name == name)
                {
                    StartCoroutine(FadeMusic(s.source,1f,0.3f));
                    s.source.Play();
                }
            }
        }
        public void Stop(string name)
        {
            foreach (var s in sounds)
            {
                if (s == null) return;

                if (s.name == name)
                {
                    StartCoroutine(FadeMusic(s.source,1f,0f));
                    if(s.source.volume == 0)
                    s.source.Stop();
                }
            }
        }

        public void PlayOneShot(string name)
        {
            foreach (var s in sounds)
            {
                if (s == null) return;

                if (s.name == name)
                {
                    s.source.PlayOneShot(s.clip);
                }
            }
        }

        public IEnumerator FadeMusic(AudioSource source, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = source.volume;
            while(currentTime < duration)
            {
                currentTime += Time.deltaTime;
                source.volume = Mathf.Lerp(start, targetVolume, currentTime/duration);
                yield return null;
            }
            yield break;
        }

    }
}
