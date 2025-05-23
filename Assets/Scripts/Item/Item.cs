using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// 상호작용 프롬프트를 Get
    /// </summary>
    /// <returns></returns>
    public string GetInteractPrompt();
    /// <summary>
    /// 상호 작용 실행
    /// </summary>
    public void OnInteract();
}
/// <summary>
/// 아이템
/// </summary>
public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.player.itemData = data;
        CharacterManager.Instance.player.OnAddItem?.Invoke();
        Destroy(gameObject);
    }
}
