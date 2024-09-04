using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple_Move : MonoBehaviour
{
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("RotateCamera")]
    public float mouseSpeed;
    float yRotation;
    float xRotation;
    [SerializeField]
    Camera cam;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        // Reset velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        RotateCamera();
    }

    void RotateCamera()
    {

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        yRotation += mouseX; // ���콺 x�� �Է¿� ���� ���� ȸ�� ���� ����
        xRotation -= mouseY; // ���콺 y�� �Է¿� ���� ���� ȸ�� ���� ����

        //xRotation = Mathf.Clamp(xRotation, -90f, 80f); //���� ȸ�� ���� -90~90���� ����. (��, �Ʒ�)

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
