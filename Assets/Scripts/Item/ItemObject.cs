using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    // ������ ��ũ���ͺ� �־��� �� 
    public ItemData data;


    // �Ʒ��� ��ȣ�ۿ� �� �� �ʿ��� �ڵ�� 
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.des}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }


}
