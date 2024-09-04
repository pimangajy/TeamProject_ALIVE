using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vrPlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2f;
    private InputManager inputManager;
    private CharacterController characterController;

    private Vector3 velocity;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81f;

    //[SerializeField]
    //Camera camera;
    //float lookSensitivity = 1f; // 마우스 민감도

    Rigidbody rb;
    public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();

        if (inputManager == null)
        {
            Debug.LogError("InputManager component not found on " + gameObject.name);
        }

        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on " + gameObject.name);
        }

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck transform not assigned in the inspector");
        }

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();  // 플레이어 인풋
    }

    //private void FixedUpdate()
    //{
    //    rb.velocity = direction * moveSpeed * Time.deltaTime;

    //}

    private void PlayerMove()
    {
        if (groundCheck == null || inputManager == null || characterController == null)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float speed = inputManager.runInput ? runSpeed : (inputManager.crouchInput ? crouchSpeed : walkSpeed);
        Vector3 move = transform.right * inputManager.movementInput.x + transform.forward * inputManager.movementInput.y;
        characterController.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    //private void CameraRotation()
    //{
    //    float xRotation = Input.GetAxisRaw("Mouse Y");
    //    float cameraLotX = xRotation * lookSensitivity;

    //    camera.transform.localEulerAngles = new Vector3(cameraLotX, 0, 0);   
    //}
}
