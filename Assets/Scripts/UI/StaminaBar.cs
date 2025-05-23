/// <summary>
/// 스테미너 바
/// </summary>
public class StaminaBar : GaugeBar
{
    public override void SetValue(float value)
    {
        FillImage.fillAmount = value;
    }
}