using UnityEngine;
using UnityEngine.UI;

public abstract class GaugeBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    protected Image FillImage => fillImage;

    public abstract void SetValue(float value);
}
