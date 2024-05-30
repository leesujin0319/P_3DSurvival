using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    // ���� �ð�
    public float attackRate;
    private bool attacking;
    // �ִ� ���� �Ÿ�
    public float attackDistacne;
    public float useStamina;

    [Header("Resource Gathering")]
    // ���ҽ��� ������ �ִ��� bool �� 
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
            // ���⼭ ������ �Ѵٸ� stamina ���̱� 
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
