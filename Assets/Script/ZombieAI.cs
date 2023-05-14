using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Trace,
        Attack,
        Dead
    };

    public State curState;
    
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    public float health;

    public bool canAttack;
    public float attackTime;
    public float attackRange;

    public bool isDead;
    
    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(CheckState());
        StartCoroutine(ChangeStateAction());
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = agent.velocity.normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void Move()
    {
        agent.SetDestination(player.position);
    }

    private IEnumerator CheckState()
    {
        while (!isDead)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= attackRange)
            {
                curState = State.Attack;
                yield return new WaitForSeconds(4f);
            }
            else
            {
                curState = State.Trace;
                yield return new WaitForSeconds(1.4f);
            }
        }
    }

    private IEnumerator ChangeStateAction()
    {
        float waitTime;
        while (!isDead)
        {
            switch (curState)
            {
                case State.Trace:
                    agent.SetDestination(player.position);
                    animator.SetInteger("State", (int)curState);
                    break;
                case State.Attack:
                    if (canAttack)
                    {
                        StartCoroutine(Attack());
                        animator.SetInteger("State", (int)curState);
                    }
                    break;
            }

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log("Attack");
        canAttack = false;

        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }
}
