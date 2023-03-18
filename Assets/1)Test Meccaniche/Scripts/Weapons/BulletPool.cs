using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheChroniclesOfEllen
{
    public class BulletPool : MonoBehaviour
    {
        private static Bullet[] bullets;
        private static Bullet[] bulletsSpecial;
        public int poolSize;
        public int poolSpecialSize;
        public Bullet bulletPrefab;
        public Bullet bulletSpecialPrefab;
      

        // Start is called before the first frame update
        void Awake()
        {
            InitPool();
        }

        private void InitPool()
        {
            //TODO: vedere come fare per i tipi di proiettile
            bullets = new Bullet[poolSize];
            bulletsSpecial = new Bullet[poolSpecialSize];
            Bullet go;

            for(int i = 0; i < poolSize; i++)
            {
                go = Instantiate<Bullet>(bulletPrefab);
                go.gameObject.SetActive(false);
                bullets[i] = go;
            }
            for(int i = 0; i < poolSpecialSize; i++)
            {
                go = Instantiate<Bullet>(bulletSpecialPrefab);
                go.gameObject.SetActive(false);
                bulletsSpecial[i] = go;
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
        public static Bullet GetBulletSpecial()
        {
            for (int i = 0; i < bulletsSpecial.Length; i++)
            {
                if (!bulletsSpecial[i].gameObject.activeInHierarchy)
                {
                    bulletsSpecial[i].gameObject.SetActive(true);
                    return bulletsSpecial[i];
                }
            }
            return null;
        }

    }
}
