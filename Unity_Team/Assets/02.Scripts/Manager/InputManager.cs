using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 movementInput { get; private set; }
    public bool runInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool crouchInput { get; private set; }
    public bool attackInput { get; private set; }
    public int selectedWeapon { get; private set; } // ���� �ε����� ���� ���ο� �Ӽ� �߰�
    public bool useItemInput { get; private set; }
    public bool collectItemInput { get; private set; }
    public bool OpenBackPack { get; private set; }


    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        runInput = Input.GetKey(KeyCode.LeftShift);
        jumpInput = Input.GetButtonDown("Jump");
        crouchInput = Input.GetKey(KeyCode.C);
        attackInput = Input.GetButtonDown("Fire1");

        // ���� Ű �Է� Ȯ��
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWeapon = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) selectedWeapon = 2;

        useItemInput = Input.GetKeyDown(KeyCode.E);
        collectItemInput = Input.GetKeyDown(KeyCode.F);
        OpenBackPack = Input.GetKeyDown(KeyCode.Tab);
    }
}