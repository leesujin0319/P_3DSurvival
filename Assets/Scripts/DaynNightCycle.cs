using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaynNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]

    public float time;
    public float fullDayLength;
    // 0.5�϶� ���� ���� 90�� �ð��� 12�ö�� ����
    public float startTime = 0.4f;
    private float timeRate;
    // Vector 90 0 0 �� �� 
    public Vector3 noon;


    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensitiy;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensitiy;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensitiyMultiplier;
    public AnimationCurve reflectionIntensitiyMultiplier;


    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensitiy);
        UpdateLighting(moon, moonColor, moonIntensitiy);

        RenderSettings.ambientIntensity = lightingIntensitiyMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensitiyMultiplier.Evaluate(time);
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        // ���� ��ȭ 
        // 0.5�� ���� �̶� ���� 90�� 360�� �϶� 180���� �����ϱ� 0.25 ���ָ� 90���� ����
        // 0.75�� ���϶� ��� 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        // �ð��� �ش�Ǵ� ������ ���� �����ϰ� ��
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0f && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
