using System;
using UnityEngine;

public enum ItemType
{
    Equipable,Consumable,Resource
}
public enum ConsumableType
{
    Health,
    Stamina,
    MoveSpeed,
    JumpCount
}
[Serializable]
public class ItemDataConsumable
{
    public bool isImmediately;
    public float time;
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item",menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}
