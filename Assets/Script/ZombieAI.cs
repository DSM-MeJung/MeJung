using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    public float health;

    private bool isWalk;
    private bool isDead;
    private bool canAttack = true;

    public float attackTime;
    public float attackRange;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                if (canAttack)
                {
                    StartCoroutine(Attack());
                }
                else
                {
                    animator.SetBool("isWalk", false);
                }
            }
            else
            {
                agent.SetDestination(player.position);
                animator.SetBool("isWalk", true);
            }
        }
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

    private IEnumerator Attack()
    {
        Debug.Log("Attack");
        canAttack = false;
        agent.ResetPath();
        animator.SetBool("canAttack", true);
        animator.SetBool("isWalk", false);
        
        yield return new WaitForSeconds(attackTime);
        
        animator.SetBool("canAttack", false);
        canAttack = true;
    }
}
