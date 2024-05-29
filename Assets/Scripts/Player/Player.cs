using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    // 여기에 상호작용된 데이터를 넘겨줄거임
    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        // 나 자신을 집어넣기 
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
