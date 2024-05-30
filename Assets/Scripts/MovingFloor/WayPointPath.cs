using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class WayPointPath : MonoBehaviour
{
   public Transform GetWayPoint(int waypointIndex)
    {
        // �ڽ� ������Ʈ�� �ε��� ��������
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWayPointIndex(int curWaypointIndex)
    {
        int nextWayPointIndex = curWaypointIndex + 1;

        // ���� �ε����� �ڽ� �ε����� ���ٸ� 
        if(nextWayPointIndex == transform.childCount)
        {
            // �ٽ� �ʱ�ȭ
            nextWayPointIndex = 0;
        }

        return nextWayPointIndex;
    }
}
