using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action OnTakeDamage;

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public IEnumerator Running()
    {
        UseStamina(20);
        float originalSpeed = CharacterManager.Instance.Player.controller.moveSpeed;
        float upgrageSpeed = CharacterManager.Instance.Player.controller.moveSpeed * 2f; 

        CharacterManager.Instance.Player.controller.moveSpeed = upgrageSpeed;

        yield return new WaitForSeconds(5f);

        CharacterManager.Instance.Player.controller.moveSpeed = originalSpeed;  
    }

    public void UseRun()
    {
        StartCoroutine(Running());
    }

    public void Die()
    {
        Debug.Log("죽었다!");
    }

    public void TakePhysicalDamage(int damage)
    {
        // 데미지 만큼 피 깎기
        health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }
}
