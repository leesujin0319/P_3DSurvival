using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    // �ð����� �� �ִ� ��ũ��Ʈ 
    public Image image;
    public float flashSpeed;

    // �ڷ�ƾ ���� ����
    private Coroutine coroutine;

    private void Start()
    {
        // �÷��̾�� Flash ������
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
        // �ڷ�ƾ ���� 
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        // �ѹ� ������ ������ �������� �� 
        while (a > 0)
        {
            // ���İ���ŭ ������ �� ��������
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }

        // while�� ������ �̹��� �ٽ� ���ֱ� 
        image.enabled = false;
    }
}
