using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp = 10f;

    public float invicibleTime;
    public IHealthListener healthListener;
    public Image hpGuage;
    public AudioClip hitSound;
    public AudioClip dieSound;

    float lastAttackedTime;
    float maxHP;


    void Start()
    {
        healthListener = GetComponent<Health.IHealthListener>();

        maxHP = hp;
    }


    void Update()
    {

    }


    public void Damage(float damage)
    {
        if (hp > 0 && lastAttackedTime + invicibleTime < Time.time)
        {
            hp -= damage;
            if (hpGuage != null)
            {
                hpGuage.fillAmount = hp / maxHP;
            }

            lastAttackedTime = Time.time;

            if (hp <= 0)
            {
                if (dieSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                }

                if (healthListener != null)
                {
                    healthListener.Die();
                }
            }
            else
            {
                if (hitSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(hitSound);
                }
            }
        }
    }


    public interface IHealthListener
    {
        void Die();
    }
}
