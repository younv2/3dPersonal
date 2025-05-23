/// <summary>
/// HP바 UI
/// </summary>
public class HpBar : GaugeBar
{
    public override void SetValue(float value)
    {
        FillImage.fillAmount = value;
    }
}
