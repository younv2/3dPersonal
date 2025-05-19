using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStat stat = new();
    private void Awake()
    {
        CharacterManager.Instance.player = this;
        controller = GetComponent<PlayerController>();
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
