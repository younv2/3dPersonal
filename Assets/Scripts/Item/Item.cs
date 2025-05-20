using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();

    public void OnInteract();
}
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
