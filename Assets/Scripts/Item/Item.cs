using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// ��ȣ�ۿ� ������Ʈ�� Get
    /// </summary>
    /// <returns></returns>
    public string GetInteractPrompt();
    /// <summary>
    /// ��ȣ �ۿ� ����
    /// </summary>
    public void OnInteract();
}
/// <summary>
/// ������
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
