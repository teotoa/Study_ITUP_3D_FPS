using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;
    public float damage = 10f;
    public AudioClip exlosionSound;


    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(exlosionSound);
            }

            GetComponent<Animator>().SetTrigger("Explosion");
            Invoke("DestroyThis", 3f);
        }
    }


    void DestroyThis()
    {
        Destroy(gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Health>().Damage(damage);
        }
    }
}
