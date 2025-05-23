using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �κ��丮 UI
/// </summary>
public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;

    public Transform dropPosition;
    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerStat stat;

    ItemData selectedItem;
    int selectedItemIndex = 0;

    int curEquipIndex;

    // �κ��丮 �ʱ� ����
    private void Start()
    {
        Player player = CharacterManager.Instance.player;
        controller = player.controller;
        stat = player.stat;
        dropPosition = player.dropPosition;
        player.OnAddItem += AddItem;
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        controller.InputHandler.onInventoryAction += Toggle;
        ClearSelectedItemWindow();
        useButton.GetComponent<Button>().onClick.AddListener(() => OnUseButton());
        dropButton.GetComponent<Button>().onClick.AddListener(() => OnDropButton());
        equipButton.GetComponent<Button>().onClick.AddListener(() => OnEquipButton());
        unequipButton.GetComponent<Button>().onClick.AddListener(() => OnUnEquipButton());
    }
    //�̺�Ʈ ����
    private void OnDestroy()
    {
        controller.InputHandler.onInventoryAction -= Toggle;
        if(CharacterManager.Instance!=null)
        CharacterManager.Instance.player.OnAddItem -= AddItem;
    }
    /// <summary>
    /// ������ â ����
    /// </summary>
    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    /// <summary>
    /// ������ â Ȱ��ȭ ���
    /// </summary>
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }
    /// <summary>
    /// ������ â �����ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
    /// <summary>
    /// ������ �߰� �Լ�
    /// </summary>
    private void AddItem()
    {
        ItemData data = CharacterManager.Instance.player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.player.itemData = null;
            return;
        }
        ThrowItem(data);
        CharacterManager.Instance.player.itemData = null;
    }
    /// <summary>
    /// UI�� ������Ʈ ���ִ� �Լ�
    /// </summary>
    private void UpdateUI()
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
    /// <summary>
    /// ������ ���� �Լ�
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }
    /// <summary>
    /// ����ִ� ������ �޾ƿ��� �Լ�
    /// </summary>
    /// <returns></returns>
    private ItemSlot GetEmptySlot()
    {
        for(int i =0; i<slots.Length;i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }
    /// <summary>
    /// ������ ������
    /// </summary>
    /// <param name="data"></param>
    private void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab,dropPosition.position,Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }
    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="index"></param>
    public void SelectedItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for(int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);

    }
    /// <summary>
    /// ������ ��� �̺�Ʈ
    /// </summary>
    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                stat.ApplyConsumableEffect(selectedItem.consumables[i]);
            }
            RemoveSelectedItem();
        }
    }
    /// <summary>
    /// ������ ��� �̺�Ʈ
    /// </summary>
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }
    /// <summary>
    /// ���� ������ ����
    /// </summary>
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        if (slots[selectedItemIndex].quantity <= 0 )
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }
    /// <summary>
    /// ������ ���� �̺�Ʈ
    /// </summary>
    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);

        }
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            switch (selectedItem.consumables[i].type)
            {
                case ConsumableType.MoveSpeed:
                    stat.SetMoveEquip(selectedItem.consumables[i].value);
                    break;
            }
        }
        CharacterManager.Instance.player.equip.EquipNew(selectedItem);

        UpdateUI();

        SelectedItem(selectedItemIndex);
    }
    /// <summary>
    /// ������ ��������
    /// </summary>
    /// <param name="index"></param>
    void UnEquip(int index)
    {
        slots[index].equipped = false;

        // ���� ��� ���� ���� ȿ�� ����
        ItemData equippedItem = slots[index].item;
        for (int i = 0; i < equippedItem.consumables.Length; i++)
        {
            switch (equippedItem.consumables[i].type)
            {
                case ConsumableType.MoveSpeed:
                    stat.SetMoveEquip(0); // �Ǵ� �⺻������ �ʱ�ȭ
                    break;
            }
        }

        CharacterManager.Instance.player.equip.UnEquip();
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectedItem(selectedItemIndex);
        }
    }
    /// <summary>
    /// ������ ���� ���� �̺�Ʈ
    /// </summary>
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
