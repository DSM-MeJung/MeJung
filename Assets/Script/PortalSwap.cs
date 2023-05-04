using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PortalSwap : MonoBehaviour
{
    [SerializeField]private GameObject[] portal;

    private void Awake()
    {
        portal = GameObject.FindGameObjectsWithTag("Portal");

        for (int i = 0; i < 103; i++)
        {
            var pos1 = Random.Range(0, portal.Length);
            var pos2 = Random.Range(0, portal.Length);
            (portal[pos1].transform.position, portal[pos2].transform.position) = (portal[pos2].transform.position, portal[pos1].transform.position);
        }
    }
}
