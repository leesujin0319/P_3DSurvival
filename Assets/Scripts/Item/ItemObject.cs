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
    // 아이템 스크립터블 넣어줄 것 
    public ItemData data;


    // 아래는 상호작용 할 때 필요한 코드들 
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
