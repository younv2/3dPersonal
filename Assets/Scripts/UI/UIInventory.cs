using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update
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
    private void OnDestroy()
    {
        controller.InputHandler.onInventoryAction -= Toggle;
        if(CharacterManager.Instance!=null)
        CharacterManager.Instance.player.OnAddItem -= AddItem;
    }
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
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

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
    private void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab,dropPosition.position,Quaternion.Euler(Vector3.one * UnityEngine.Random.value * 360));
    }

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
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }
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

    void UnEquip(int index)
    {
        slots[index].equipped = false;

        // 기존 장비에 의한 스탯 효과 제거
        ItemData equippedItem = slots[index].item;
        for (int i = 0; i < equippedItem.consumables.Length; i++)
        {
            switch (equippedItem.consumables[i].type)
            {
                case ConsumableType.MoveSpeed:
                    stat.SetMoveEquip(0); // 또는 기본값으로 초기화
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
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
