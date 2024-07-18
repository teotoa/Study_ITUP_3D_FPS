using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour, Health.IHealthListener
{
    enum State { Idle, Walk, Attack, Dying }

    public float timeForNextState = 2f;

    public GameObject player;

    NavMeshAgent agent;
    Animator animator;
    State state;
    AudioSource audio;



    void Start()
    {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = State.Idle;
    }


    void Update()
    {
        switch (state)
        {
            case State.Idle:
                float distance = (player.transform.position -
                    (transform.position + GetComponent<CapsuleCollider>().center)).magnitude;

                if (distance < 1.0f)
                {
                    Attack();
                }
                else
                {
                    timeForNextState -= Time.deltaTime;
                    if (timeForNextState < 0)
                    {
                        StartWalk();
                    }
                }
                break;

            case State.Walk:
                if (agent.remainingDistance < 1.0f || !agent.hasPath)
                {
                    StartIdle();
                }
                break;

            case State.Attack:
                timeForNextState -= Time.deltaTime;
                if (timeForNextState < 0)
                {
                    StartIdle();
                }
                break;
        }

        if (timeForNextState < 0)
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Walk:
                    break;
            }
        }
    }


    void Attack()
    {
        state = State.Attack;
        timeForNextState = 1.5f;
        animator.SetTrigger("Attack");
    }


    void StartWalk()
    {
        audio.Play();
        state = State.Walk;
        agent.destination = player.transform.position;
        agent.isStopped = false;
        animator.SetTrigger("Walk");
    }


    void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        timeForNextState = Random.Range(1f, 2f);
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }


    public void Die()
    {
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis", 2.5f);
    }


    void DestroyThis()
    {
        GameManager.instance.EnemyDied();
        Destroy(gameObject);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(5);
        }
    }
}
