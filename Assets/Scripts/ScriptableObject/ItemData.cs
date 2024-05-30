using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource, // 단순 자원 ex) 돌 

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
    // 얼마만큼 회복 되는지
    public float value;
}


// 메뉴창에 빠르게 추가하기 위해 생성
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
    // 여러개를 가질 수 있는 아이템인지 
    public bool canStack;
    // 얼마나 가질 수 있는지
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;


    [Header("Equip")]
    public GameObject equipPrefab;
}
