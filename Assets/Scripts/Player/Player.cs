using System;
using UnityEngine;
/// <summary>
/// 플레이어 클래스(플레이어 총괄 관리)
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerStat stat = new();
    public Equipment equip;
    public ItemData itemData;
    public Action OnAddItem;

    public Transform dropPosition;
    private void Awake()
    {
        CharacterManager.Instance.player = this;
        controller = GetComponent<PlayerController>();
        equip = GetComponent<Equipment>();
    }
    private void Start()
    {
        controller.Init(stat);
        stat.OnHpChanged += (ctx) => UIManager.Instance.HpBar.SetValue(ctx);
        stat.OnStaminaChanged += (ctx) => UIManager.Instance.StaminaBar.SetValue(ctx);
    }
    private void Update()
    {
        stat.SpendHp(0.01f);
    }
}
