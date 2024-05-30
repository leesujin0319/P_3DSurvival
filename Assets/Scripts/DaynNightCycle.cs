using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaynNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]

    public float time;
    public float fullDayLength;
    // 0.5일때 정오 각도 90도 시간은 12시라고 생각
    public float startTime = 0.4f;
    private float timeRate;
    // Vector 90 0 0 인 값 
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

        // 각도 변화 
        // 0.5가 정오 이땐 각도 90도 360도 일때 180도가 정오니까 0.25 빼주면 90도가 나옴
        // 0.75도 밤일때 경우 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        // 시간에 해당되는 일정한 값을 추출하게 됨
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
