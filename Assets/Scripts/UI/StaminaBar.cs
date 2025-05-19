public class StaminaBar : GaugeBar
{
    public override void SetValue(float value)
    {
        FillImage.fillAmount = value;
    }
}