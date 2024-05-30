using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    // 공격 시간
    public float attackRate;
    private bool attacking;
    // 최대 공격 거리
    public float attackDistacne;
    public float useStamina;

    [Header("Resource Gathering")]
    // 리소스를 가지고 있는지 bool 값 
    public bool doseGatheringResource;

    [Header("Combat")]
    public bool doseDealDamege;
    public int damage;

    private Animator animator;
    private Camera camera;

    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            // 여기서 공격을 한다면 stamina 깎이기 
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }

        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    //public void Onhit()
    //{
    //    Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, attackDistacne))
    //    {
    //        if (doseGatheringResource && hit.collider.TryGetComponent(out Resource resource))
    //        {
    //            resource.Gather(hit.point, hit.normal);
    //        }
    //    }
    //}

    public override void OnUpgradeJump()
    {
        CharacterManager.Instance.Player.controller.jumpPower = 100f;
    }
}
