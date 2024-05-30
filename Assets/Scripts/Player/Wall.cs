using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float rayDistance = 1f; // Raycast의 거리
    public LayerMask wallLayer; // 벽 레이어
    public float climbForce = 10f; // 벽을 타기 위한 힘
    public float clingDuration = 2f; // 매달리는 시간
    private bool isClinging = false; // 매달리고 있는지 여부
    private Rigidbody rb; // Rigidbody 컴포넌트
    private float clingStartTime; // 매달리기 시작한 시간

    void Start()
    {
        rb = CharacterManager.Instance.Player.GetComponent<Rigidbody>();
        wallLayer = LayerMask.GetMask("Wall"); // "Wall" 레이어 설정
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isClinging)
        {
            CheckForWall();
        }

        if (isClinging && Time.time - clingStartTime > clingDuration)
        {
            isClinging = false;
            rb.useGravity = true;
        }
    }

    void CheckForWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, wallLayer))
        {
            if (hit.collider != null)
            {
                ClingToWall(hit.point);
            }
        }
    }

    void ClingToWall(Vector3 hitPoint)
    {
        isClinging = true;
        clingStartTime = Time.time;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.AddForce((hitPoint - transform.position).normalized * climbForce, ForceMode.Impulse);
    }
}
