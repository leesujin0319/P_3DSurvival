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
        // AddItem 등록
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        // slotPanel 아래에 14개의 슬롯 길이 ( 자식의 갯수 )
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

    // 탭 키를 누르면 창이 꺼지고 켜지는 UI 효과
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
        // 하이어라키에 활성화 되어있는지 아닌지
        return inventoryWindow.activeInHierarchy;
    }

    // 상호작용을 했을 때 AddItem이 등록이 되어있다면
    void AddItem()
    {
        // 플레이어에게 아이템 데이터 넣어 놨음
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 아이템 중복 가능한지 >> canStack 
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
        // 비어있는 슬롯을 가져온다
        ItemSlot emptySlot = GetEmptySlot();

        // 비어있는 슬롯이 있다면
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
        }
        // 비어있는 슬롯이 없다면 아이템을 버리기 
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
            // 슬롯이 데이터가 같고 12개이상일때 그냥 원래껄로 반환해라 
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
        // dropitem이 랜덤으로 떨어지게 
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

        // 스탯이 있는 아이템인 경우 
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            // 다음꺼가 있다면 한 줄 띄워야함
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
        // 타입이 consumable일때만 아이템 사용가능
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
        // 정보창에서 없애기
        RemoveSelectedItem();
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        // 버리면 갯수 차감
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
        // 장착되어있으면
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
