using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed = 500.0f;
    private float xRotate, yRotate, xRotateMove, yRotateMove;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        CameraMove();
    }
    
       private void CameraMove()
        {
            xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
            yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
    
            yRotate = transform.eulerAngles.y + yRotateMove;
            xRotate = xRotate + xRotateMove;
                
            xRotate = Mathf.Clamp(xRotate, -90, 90);
                
            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }

       /* Move use transform

       private void Move()
       {
           if(Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
           if(Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
           if(Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
           if(Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
       }
       */


    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(inputX, 0, inputZ);
        moveDir = transform.TransformDirection(moveDir);
        moveDir *= moveSpeed * Time.deltaTime;
        characterController.Move(moveDir);
    }
    
    // private void Move()
    // {
    //     float horizontal = Input.GetAxis("Horizontal");
    //     float vertical = Input.GetAxis("Vertical");
    //     
    //     Vector3 moveDir = new Vector3(-vertical, 0, horizontal);
    //     
    //     moveDir *= moveSpeed;
    //
    //     characterController.Move(moveDir * Time.deltaTime);
    // }
}
