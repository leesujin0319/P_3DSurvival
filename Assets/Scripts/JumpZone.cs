using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    private Rigidbody rigidbody;
    private float power = 400f;

    private void Start()
    {
        rigidbody = CharacterManager.Instance.GetComponent<Rigidbody>();
       
    }


    private void OnCollisionEnter(Collision collision)
    {
        CharacterManager.Instance.Player.controller.jumpPower = power;

        if (collision.gameObject.tag=="JumpZone")
        {
            rigidbody.AddForce(Vector2.up *power, ForceMode.Impulse);
        }
    }
}
