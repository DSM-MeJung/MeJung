using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;
using UniRx.Triggers;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    
    [SerializeField] private float moveSpeed;
    private float gravity = 20f;
    [SerializeField] private Camera _camera;
    [SerializeField] private float rotateSpeed = 500.0f;
    private float xRotate;
    private float yRotate;
    private float xRotateMove;
    private float yRotateMove;
    private Vector2 inputPos;
    [SerializeField]private Animator animator;
    
    enum animState
    {
        right = 1,
        left,
        front,
        back,
        idle,
        meleeAttack,
        torchAttack
    }


    private void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;   
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        CameraMove();
        State();
    }



    private void CameraMove()
    {
        xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
        yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

        yRotate = transform.eulerAngles.y + yRotateMove;
        xRotate += xRotateMove;

        xRotate = Mathf.Clamp(xRotate, -40, 50);

        _camera.transform.eulerAngles = new Vector3(xRotate, transform.eulerAngles.y, 0);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
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
        inputPos.x = Input.GetAxis("Horizontal");
        inputPos.y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(inputPos.x, 0, inputPos.y);
        moveDir = transform.TransformDirection(moveDir);
        moveDir *= moveSpeed * Time.deltaTime;
        moveDir.y -= gravity * Time.deltaTime;
        characterController.Move(moveDir);
    }

    private void State()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            animator.SetInteger("AnimationState", (int)animState.meleeAttack);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetInteger("AnimationState", (int)animState.torchAttack);
        }   
        else if (inputPos.x > 0)
        {
            animator.SetInteger("AnimationState", (int)animState.right);
        }   
        else if (inputPos.x < 0)
        {
            animator.SetInteger("AnimationState", (int)animState.left);
        }
        else if (inputPos.y > 0)
        {
            animator.SetInteger("AnimationState", (int)animState.front);
        }
        else if (inputPos.y < 0)
        {
            animator.SetInteger("AnimationState", (int)animState.back);
        }
        else
        {
            animator.SetInteger("AnimationState", (int)animState.idle);
        }
        
    }
}
