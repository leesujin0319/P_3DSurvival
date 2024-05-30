using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // Bar ��ȭ�ϴ� ��ũ��Ʈ 
    public float curValue;
    public float startValue;
    public float maxValue;
    // �ֱ������� ���ϴ� �� 
    public float passiveValue;
    // �̹����� �ִ� fillamount �� ������ �ű� ������
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        // UI ������Ʈ 
        uiBar.fillAmount = GetPercentage();
    }


    float GetPercentage()
    {
        // fillAmount ���� 0~1 �̴ϱ� 
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        // �ּڰ� ���ؼ� ���ϱ� ��ġ�� max�� �ѱ��� �ʰ� 
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
