using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBulletEffects : MonoBehaviour
{
    [SerializeField]
    private  ParticleSystem explosion;
    [SerializeField]
    private  ParticleSystem lightning;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" || other.tag == "Boss" || other.tag == "Ground")
        {
            transform.localPosition = other.transform.position;
            var explosionDuration = explosion.main.duration;
            explosion.Play();
            lightning.Play();
            
            Invoke(nameof(DeactivateBullet),explosionDuration);
        }
    }

    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }
}
