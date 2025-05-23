using System;
using UnityEngine;


[Serializable]
public class ItemDataConsumable
{
    public bool isImmediately;
    public float time;
    public ConsumableType type;
    public float value;
}
/// <summary>
/// 아이템 데이터를 저장하는 SO
/// </summary>
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
