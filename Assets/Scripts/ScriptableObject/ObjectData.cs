using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Object
}


// 메뉴창에 빠르게 추가하기 위해 생성
[CreateAssetMenu(fileName = "Object", menuName = "new Object")]
public class ObjectData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string des;
    public ObjectType type;
}

