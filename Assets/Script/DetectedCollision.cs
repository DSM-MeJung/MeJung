using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedCollision : MonoBehaviour
{
    private CapsuleCollider collider;

    private void Start()
    {
        collider = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected. {collision.gameObject.name}");
    }
}
