using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    
    [SerializeField] private float moveSpeed;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 moveDir = new Vector3(-vertical, 0, horizontal);
        
        moveDir *= moveSpeed;

        characterController.Move(moveDir * Time.deltaTime);
    }
}
