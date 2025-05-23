using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ItemSlot 클래스
/// </summary>
public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public UIInventory inventory;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    private void Start()
    {
        button.onClick.AddListener(() => OnClickButton());
    }
    private void OnEnable()
    {
        outline.enabled = equipped;
    }
    /// <summary>
    /// 아이템 슬롯 아이템 데이터에 맞게 초기 세팅
    /// </summary>
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if(outline != null)
        {
            outline.enabled = equipped;
        }
    }
    /// <summary>
    /// 아이템 슬롯 초기화
    /// </summary>
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }
    /// <summary>
    /// 슬롯 클릭 이벤트
    /// </summary>
    public void OnClickButton()
    {
        inventory.SelectedItem(index);
    }
}
