using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    // 경로 
    private WayPointPath _waypointPath;

    [SerializeField]
    private float _speed;

    // 다음 목표 경로
    private int _targetWayPointIndex;

    // 이전 경로 
    private Transform _previousWaypoint;
    // 다음 목표
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
        
        // 시간 계산
        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        // 스무스하게 움직이게 
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);


        // 1 이상이면 다음 경로 지점 설정
        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint()
    {
        // 현재 경로 지점 
        _previousWaypoint = _waypointPath.GetWayPoint(_targetWayPointIndex);
        // 다음 경로 지점 
        _targetWayPointIndex = _waypointPath.GetNextWayPointIndex(_targetWayPointIndex);
        _targetWaypoint = _waypointPath.GetWayPoint(_targetWayPointIndex);

        _elapsedTime = 0;

        // 이전 경로 지점과 다음 경로 지점의 거리를 계산 
        float distanceToWayPoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        // 속도에 따라 이동 시간을 계산 
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
