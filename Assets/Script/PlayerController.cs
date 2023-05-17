using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;
using UniRx.Triggers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Space] [Header("HP")]
    [SerializeField] private int hp;
    [SerializeField]   private Slider hpSlider;

    [SerializeField] private GameObject meleeBox;
    [SerializeField] private GameObject torchBox;
    
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
        hpSlider.value = hp;
        Cursor.lockState = CursorLockMode.Locked;   
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        CameraMove();
        State();
        if (Input.GetKeyDown(KeyCode.P))
            Damage(5);
    }

    void Damage(int value)
    {
        hp-=value;
        if(hp <= 0)
            Debug.Log("Die!");
        hpSlider.value = hp;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            SceneManager.LoadScene("GameEnd");
        }

        if (other.CompareTag("Monster"))
        {
            switch (other.name)
            {
                case "Swamp":
                    Damage(5);
                    break;
            }
        }
    }

    public void StartMeleeAttack()
    {
        meleeBox.SetActive(true);
    }
    public void DoneMeleeAttack()
    {
        meleeBox.SetActive(false);
    }
    public void StartTorchAttack()
    {
        torchBox.SetActive(true);
    }
    public void DoneTorchAttack()
    {
        torchBox.SetActive(false);
    }
}
