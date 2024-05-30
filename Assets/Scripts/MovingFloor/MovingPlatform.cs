using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    // ��� 
    private WayPointPath _waypointPath;

    [SerializeField]
    private float _speed;

    // ���� ��ǥ ���
    private int _targetWayPointIndex;

    // ���� ��� 
    private Transform _previousWaypoint;
    // ���� ��ǥ
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;

    private void Start()
    {
        TargetNextWaypoint();
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        
        // �ð� ���
        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        // �������ϰ� �����̰� 
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);


        // 1 �̻��̸� ���� ��� ���� ����
        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        // ���� ��� ���� 
        _previousWaypoint = _waypointPath.GetWayPoint(_targetWayPointIndex);
        // ���� ��� ���� 
        _targetWayPointIndex = _waypointPath.GetNextWayPointIndex(_targetWayPointIndex);
        _targetWaypoint = _waypointPath.GetWayPoint(_targetWayPointIndex);

        _elapsedTime = 0;

        // ���� ��� ������ ���� ��� ������ �Ÿ��� ��� 
        float distanceToWayPoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        // �ӵ��� ���� �̵� �ð��� ��� 
        _timeToWaypoint = distanceToWayPoint / _speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform); 
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
