using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    // 시각적인 거 주는 스크립트 
    public Image image;
    public float flashSpeed;

    // 코루틴 변수 생성
    private Coroutine coroutine;

    private void Start()
    {
        // 플레이어에게 Flash 더해줌
        CharacterManager.Instance.Player.condition.OnTakeDamage += Flash;
    }

    public void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        // 코루틴 시작 
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        // 한번 켜지고 서서히 연해지는 거 
        while (a > 0)
        {
            // 알파값만큼 서서히 색 연해지기
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        // while문 끝나면 이미지 다시 꺼주기 
        image.enabled = false;
    }
}
