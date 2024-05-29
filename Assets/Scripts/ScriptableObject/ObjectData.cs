using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Object
}


// �޴�â�� ������ �߰��ϱ� ���� ����
[CreateAssetMenu(fileName = "Object", menuName = "new Object")]
public class ObjectData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string des;
    public ObjectType type;
}

