using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheChroniclesOfEllen
{
    public class BulletPool : MonoBehaviour
    {
        private static Bullet[] bullets;
        public int poolSize;
        public Bullet bulletPrefab;

        // Start is called before the first frame update
        void Awake()
        {
            InitPool();
        }

        private void InitPool()
        {
            //TODO: vedere come fare per i tipi di proiettile
            bullets = new Bullet[poolSize];
            Bullet go;

            for(int i = 0; i < poolSize; i++)
            {
                go = Instantiate<Bullet>(bulletPrefab);
                go.gameObject.SetActive(false);
                bullets[i] = go;
            }
        }

        public static Bullet GetBullet()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].gameObject.activeInHierarchy)
                {
                    bullets[i].gameObject.SetActive(true);
                    return bullets[i];
                }
            }
            return null;
        }
    }
}
