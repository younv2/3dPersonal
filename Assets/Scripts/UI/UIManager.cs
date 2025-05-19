using UnityEngine;
public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private HpBar hpBar;
    [SerializeField] private StaminaBar staminaBar;

    public HpBar HpBar { get { return hpBar; } }
    public StaminaBar StaminaBar { get { return staminaBar; } }

    protected override void Awake()
    {
        base.Awake();

    }
}
