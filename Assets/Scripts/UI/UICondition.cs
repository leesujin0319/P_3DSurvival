using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;


    // Start is called before the first frame update
    void Start()
    {
        // 인스펙터 창에는 None이지만 여기서 대신 넣어주는 거임
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
