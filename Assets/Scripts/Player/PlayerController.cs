using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 플레이어의 움직임을 담당하는 스크립트 
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    // x의 회전범위 
    public float minXLook;
    public float maxXLook;
    // 인풋액션에서 마우스의 델타값 받아오는 것 
    private float camCurXRot;
    // 회전할 때 민감도
    public float lookSensitivity;
    // 마우스를 좌우로 움직이게 되면 x값이 움직이게 됨 
    private Vector2 mouseDelta;

    // 인벤토리 커서
    public bool canLook = true;

    public Action inventory;
    private Rigidbody rd;


    private Rigidbody rb;

    private void Awake()
    {
        // 리지드바디 받아오기 
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 게임 시작할 때 마우스 커서 보이지 않게 하기 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 리지드바디 , 즉 물리연산을 하는 경우엔 FIxed를 써주는 게 좋음
    void FixedUpdate()
    {
        // 움직임 호출
        Move();
    }

    private void LateUpdate()
    {
        // true 일때만 ( 인벤이 꺼졌을때만)
        if (canLook)
        {
            // 카메라 호출
            CameraLook();
        }

    }

    // 실제 움직임 함수 
    void Move()
    {
        // 앞으로 가고 뒤로 가고 w값 ,s값 + 좌우 움직이는 값 
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rb.velocity.y;

        // 리지드바디에 방향값 넣어주기 
        rb.velocity = dir;

    }

    void CameraLook()
    {
        // y를 바꿔줘야지 좌우로 움직임
        camCurXRot += mouseDelta.y * lookSensitivity;
        // 최솟값보다 작아지면 최솟값 반환, 최댓값 반환 
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        // 로컬 좌표로 돌려주기 , -를 붙여주는 이유는 마우스를 아래로 돌리면 - 값이 나오기 때문에 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        // 위 아래로 >> 캐릭터 돌려주기 
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }


    // context에 현재 상태 받아오기 
    public void OnMove(InputAction.CallbackContext context)
    {
        // 인풋액션에 움직임이 있을 때 (Performed > 키의 움직임이 있을때 다 받아옴)
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        // 움직임이 없을 때
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // 마우스는 계속 값을 받으니까 
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            // 순간적으로 힘을 확 받는 것 
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    // Ray를 쏴서 땅에 붙어있는지 아닌지 체크하기 >> 점프 연속으로 안하려면
    // 그라운드가 아니라는 것을 인지 시켜서 더블 점프 xx
    bool isGrounded()
    {
        // 책상 다리 4개 z축 앞뒤, x축 앞뒤
        Ray[] rays = new Ray[4]
        {
            // ground에서 조금 위에서 쏘기 위해 up 
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
            // 토글 추가함
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        // 토글 커서까지 나오게 
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}
