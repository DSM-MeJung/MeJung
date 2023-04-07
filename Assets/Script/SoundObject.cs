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
        if (Vector3.Distance(destination, target.position) > 1.0f)
        {
            destination = target.position;
            agent.destination = destination;
        }
        else
        {
            PlaySound();
        }
    }

    void  PlaySound()
    {

    }
}