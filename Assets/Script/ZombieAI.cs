using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

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

    public float attackAnimationTime;
    public float walkAnimationTime;
    
    public bool isDead;

    [SerializeField] private GameObject[] armCollider;
    [SerializeField] private GameObject zombieArm;

    public SkinnedMeshRenderer skinnedMesh;
    [SerializeField]private Material skinnedMeshMaterial;
    [SerializeField]private Material dieMaterial;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.0125f;
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

    public void AttackStart()
    {
        armCollider[0].SetActive(true);
        armCollider[1].SetActive(true);
    }

  
    public void ZombieAttackStart()
    {
        zombieArm.SetActive(true);
    }
    public void ZombieAttackDone()
    {
        zombieArm.SetActive(false);
    }
    public void AttackDone()
    {
        armCollider[0].SetActive(false);
        armCollider[1].SetActive(false);
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
                yield return new WaitForSeconds(attackAnimationTime);
            }
            else
            {
                curState = State.Trace;
                yield return new WaitForSeconds(walkAnimationTime);
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

    private void Damage(int value)
    {
        health -= value;
        if (health < 0)
        {
            Die();
        }
        
    }

    private void Die()
    {
        skinnedMesh.material = dieMaterial;
        if (skinnedMesh != null)
            skinnedMeshMaterial = skinnedMesh.material;
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {  
        float counter = 0;
        while (skinnedMeshMaterial.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            skinnedMeshMaterial.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
        canAttack = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Damage(10);
        }

        if (other.CompareTag("Torch"))
        {
            Damage(5);
        }
            
    }
}
