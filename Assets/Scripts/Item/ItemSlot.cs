using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // ����â�� �� ������ ����
    public ItemData item;
    public Button button;
    public Image icon;
    public TextMeshProUGUI textMeshPro;
    private Outline outline;



    public UIInventory inventory;

    // �������� ���° �ε��� ��������
    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = true;
    }

    public void Set()
    {
        // ���Կ� �̹���, ���� ����
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        textMeshPro.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        textMeshPro.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SeletedItem(index);
    }

}
