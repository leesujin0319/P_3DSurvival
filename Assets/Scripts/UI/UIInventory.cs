using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    public TextMeshProUGUI seletedItemName;
    public TextMeshProUGUI seletedItemDes;
    public TextMeshProUGUI seletedItemStatName;
    public TextMeshProUGUI seletedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;


    private PlayerController playerController;
    private PlayerCondition playerCondition;


    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipItemIndex;

    private void Start()
    {
        playerController = CharacterManager.Instance.Player.controller;
        playerCondition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        playerController.inventory += Toggle;
        // AddItem ���
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        // slotPanel �Ʒ��� 14���� ���� ���� ( �ڽ��� ���� )
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();

    }

    void ClearSelectedItemWindow()
    {
        seletedItemName.text = string.Empty;
        seletedItemDes.text = string.Empty;
        seletedItemStatName.text = string.Empty;
        seletedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // �� Ű�� ������ â�� ������ ������ UI ȿ��
    public void Toggle()
    {
        if (isOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool isOpen()
    {
        // ���̾��Ű�� Ȱ��ȭ �Ǿ��ִ��� �ƴ���
        return inventoryWindow.activeInHierarchy;
    }

    // ��ȣ�ۿ��� ���� �� AddItem�� ����� �Ǿ��ִٸ�
    void AddItem()
    {
        // �÷��̾�� ������ ������ �־� ����
        ItemData data = CharacterManager.Instance.Player.itemData;

        // ������ �ߺ� �������� >> canStack 
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        // ����ִ� ������ �����´�
        ItemSlot emptySlot = GetEmptySlot();

        // ����ִ� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
        }
        // ����ִ� ������ ���ٸ� �������� ������ 
        ThrowItem(data);

        CharacterManager.Instance.Player.itemData = null;

    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ������ �����Ͱ� ���� 12���̻��϶� �׳� �������� ��ȯ�ض� 
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        // dropitem�� �������� �������� 
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SeletedItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        seletedItemName.text = selectedItem.displayName;
        seletedItemDes.text = selectedItem.des;

        seletedItemStatName.text = string.Empty;
        seletedItemStatValue.text = string.Empty;

        // ������ �ִ� �������� ��� 
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            // �������� �ִٸ� �� �� �������
            seletedItemStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            seletedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        // Ÿ���� consumable�϶��� ������ ��밡��
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        playerCondition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        playerCondition.Eat(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Stamina:
                        playerCondition.UseRun();
                        break; 
                }
            }
        }
        // ����â���� ���ֱ�
        RemoveSelectedItem();
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        // ������ ���� ����
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void OnEquipButton()
    {
        // �����Ǿ�������
        if (slots[curEquipItemIndex].equipped)
        {
            UnEquip(curEquipItemIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipItemIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SeletedItem(selectedItemIndex);

    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SeletedItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
