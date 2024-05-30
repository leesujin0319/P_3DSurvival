using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipMent : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController playerController;
    private PlayerCondition playerCondition;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData itemData)
    {
        UnEquip();
        curEquip = Instantiate(itemData.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        // 인벤토리가 꺼져있는 상태에서 공격가능 canLook = true 일때만 
        if (context.phase == InputActionPhase.Performed && curEquip != null && playerController.canLook)
        {
            curEquip.OnAttackInput();
            // 점프력 상승시키는 게 있다면..
            curEquip.OnUpgradeJump();
            
        }
    }
}
