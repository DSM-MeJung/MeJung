using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SoundObject : MonoBehaviour
{
    public Transform target;
    Vector3 destination;
    NavMeshAgent agent;
    AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        destination = agent.destination;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Vector3.Distance(destination, target.position) > 0.5f)
        {
            destination = target.position;
            agent.destination = destination;
        }
        else if(!audioSource.isPlaying && agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 5f)
        {
            target = gameObject.transform;
            PlaySound();
        }
    }

    void  PlaySound()
    {
        audioSource.Play();
        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}