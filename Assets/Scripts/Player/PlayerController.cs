using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // �÷��̾��� �������� ����ϴ� ��ũ��Ʈ 
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    // x�� ȸ������ 
    public float minXLook;
    public float maxXLook;
    // ��ǲ�׼ǿ��� ���콺�� ��Ÿ�� �޾ƿ��� �� 
    private float camCurXRot;
    // ȸ���� �� �ΰ���
    public float lookSensitivity;
    // ���콺�� �¿�� �����̰� �Ǹ� x���� �����̰� �� 
    private Vector2 mouseDelta;

    // �κ��丮 Ŀ��
    public bool canLook = true;

    public Action inventory;
    private Rigidbody rd;


    private Rigidbody rb;

    private void Awake()
    {
        // ������ٵ� �޾ƿ��� 
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // ���� ������ �� ���콺 Ŀ�� ������ �ʰ� �ϱ� 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // ������ٵ� , �� ���������� �ϴ� ��쿣 FIxed�� ���ִ� �� ����
    void FixedUpdate()
    {
        // ������ ȣ��
        Move();
    }

    private void LateUpdate()
    {
        // true �϶��� ( �κ��� ����������)
        if (canLook)
        {
            // ī�޶� ȣ��
            CameraLook();
        }

    }

    // ���� ������ �Լ� 
    void Move()
    {
        // ������ ���� �ڷ� ���� w�� ,s�� + �¿� �����̴� �� 
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        // ������ٵ� ���Ⱚ �־��ֱ� 
        rb.velocity = dir;

    }

    void CameraLook()
    {
        // y�� �ٲ������ �¿�� ������
        camCurXRot += mouseDelta.y * lookSensitivity;
        // �ּڰ����� �۾����� �ּڰ� ��ȯ, �ִ� ��ȯ 
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        // ���� ��ǥ�� �����ֱ� , -�� �ٿ��ִ� ������ ���콺�� �Ʒ��� ������ - ���� ������ ������ 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        // �� �Ʒ��� >> ĳ���� �����ֱ� 
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }


    // context�� ���� ���� �޾ƿ��� 
    public void OnMove(InputAction.CallbackContext context)
    {
        // ��ǲ�׼ǿ� �������� ���� �� (Performed > Ű�� �������� ������ �� �޾ƿ�)
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        // �������� ���� ��
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // ���콺�� ��� ���� �����ϱ� 
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            // ���������� ���� Ȯ �޴� �� 
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    // Ray�� ���� ���� �پ��ִ��� �ƴ��� üũ�ϱ� >> ���� �������� ���Ϸ���
    // �׶��尡 �ƴ϶�� ���� ���� ���Ѽ� ���� ���� xx
    bool isGrounded()
    {
        // å�� �ٸ� 4�� z�� �յ�, x�� �յ�
        Ray[] rays = new Ray[4]
        {
            // ground���� ���� ������ ��� ���� up 
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
         };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // ��� �߰���
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        // ��� Ŀ������ ������ 
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}
