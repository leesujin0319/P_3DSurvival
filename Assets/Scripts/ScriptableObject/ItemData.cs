using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource, // �ܼ� �ڿ� ex) �� 

}

public enum ConsumableType
{
    Health,
    Hunger,
    Stamina
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    // �󸶸�ŭ ȸ�� �Ǵ���
    public float value;
}


// �޴�â�� ������ �߰��ϱ� ���� ����
[CreateAssetMenu(fileName = "Item", menuName = "new Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string des;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    // �������� ���� �� �ִ� ���������� 
    public bool canStack;
    // �󸶳� ���� �� �ִ���
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;


    [Header("Equip")]
    public GameObject equipPrefab;
}
