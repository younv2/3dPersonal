using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������ UI(�߻� Ŭ����)
/// </summary>
public abstract class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    protected Image FillImage => fillImage;

    public abstract void SetValue(float value);
}
