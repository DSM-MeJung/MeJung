using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] monster;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(Random.Range(8f, 20f));
        if (Random.Range(0f, 10f) > 9.5f)
        {
            Instantiate(monster[Random.Range(0, monster.Length)],
                new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity); 
        }
        StartCoroutine(Spawn());
    }
}
