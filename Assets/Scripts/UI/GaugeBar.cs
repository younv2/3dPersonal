using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 게이지 UI(추상 클래스)
/// </summary>
public abstract class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    protected Image FillImage => fillImage;

    public abstract void SetValue(float value);
}
