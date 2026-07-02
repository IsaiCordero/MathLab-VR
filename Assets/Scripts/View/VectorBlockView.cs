using UnityEngine;
using TMPro;
using System.Globalization;

public class VectorBlockView : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI valueText;

    public void UpdateValue(Vector3 value)
    {
        if (valueText == null) return;

        string x = value.x.ToString("F2", CultureInfo.InvariantCulture);
        string y = value.y.ToString("F2", CultureInfo.InvariantCulture);
        string z = value.z.ToString("F2", CultureInfo.InvariantCulture);

        valueText.text = $"x: {x}\n y: {y}\n z: {z}";
    }
}