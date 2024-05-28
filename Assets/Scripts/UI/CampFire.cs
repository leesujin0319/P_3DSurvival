using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    // 데미지 
    public int damage;
    // 얼마나 자주 >> 간격
    public float damageRate;

    // 데미지를 주는 것들의 리스트
    List<IDamageable> things = new List<IDamageable>();

    private void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }
    void DealDamage()
    {
        // 지정한 데미지 만큼 깎이게 
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // 들어오면 리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    // 나가지면 리스트에서 지우기
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}
