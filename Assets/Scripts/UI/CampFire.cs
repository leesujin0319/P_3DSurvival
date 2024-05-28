using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    // ������ 
    public int damage;
    // �󸶳� ���� >> ����
    public float damageRate;

    // �������� �ִ� �͵��� ����Ʈ
    List<IDamageable> things = new List<IDamageable>();

    private void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }
    void DealDamage()
    {
        // ������ ������ ��ŭ ���̰� 
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // ������ ����Ʈ�� �߰�
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    // �������� ����Ʈ���� �����
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}
