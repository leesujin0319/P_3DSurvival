using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float rayDistance = 1f; // Raycast�� �Ÿ�
    public LayerMask wallLayer; // �� ���̾�
    public float climbForce = 10f; // ���� Ÿ�� ���� ��
    public float clingDuration = 2f; // �Ŵ޸��� �ð�
    private bool isClinging = false; // �Ŵ޸��� �ִ��� ����
    private Rigidbody rb; // Rigidbody ������Ʈ
    private float clingStartTime; // �Ŵ޸��� ������ �ð�

    void Start()
    {
        rb = CharacterManager.Instance.Player.GetComponent<Rigidbody>();
        wallLayer = LayerMask.GetMask("Wall"); // "Wall" ���̾� ����
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
