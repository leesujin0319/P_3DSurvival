using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class WayPointPath : MonoBehaviour
{
   public Transform GetWayPoint(int waypointIndex)
    {
        // 자식 오브젝트의 인덱스 가져오기
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWayPointIndex(int curWaypointIndex)
    {
        int nextWayPointIndex = curWaypointIndex + 1;

        // 현재 인덱스와 자식 인덱스가 같다면 
        if(nextWayPointIndex == transform.childCount)
        {
            // 다시 초기화
            nextWayPointIndex = 0;
        }

        return nextWayPointIndex;
    }
}
