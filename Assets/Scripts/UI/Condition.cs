using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // Bar 변화하는 스크립트 
    public float curValue;
    public float startValue;
    public float maxValue;
    // 주기적으로 변하는 값 
    public float passiveValue;
    // 이미지에 있는 fillamount 값 조정할 거기 때문에
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        // UI 업데이트 
        uiBar.fillAmount = GetPercentage();
    }


    float GetPercentage()
    {
        // fillAmount 값은 0~1 이니까 
        return curValue / maxValue;
    }

    public void Add(float value)
    {
        // 최솟값 정해서 더하기 그치만 max를 넘기지 않게 
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
