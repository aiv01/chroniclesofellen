using UnityEngine;
using UnityEngine.Audio;

public class BulletAudio : MonoBehaviour
{
   private AudioSource source;
   [SerializeField]
   private AudioClip bulletClip;
   [SerializeField]
   private AudioClip[] explosionClips;

   void Awake()
   {
     source = GetComponent<AudioSource>();
   }

   public void PlayBulletAudio()
   {
     source.loop = true;
     source.PlayOneShot(bulletClip);
     
   }

   public void PlayExplosionAudio()
   {
    source.loop = false;
    int random  = Random.Range(0,explosionClips.Length);
    source.PlayOneShot(explosionClips[random]);
   }
}
