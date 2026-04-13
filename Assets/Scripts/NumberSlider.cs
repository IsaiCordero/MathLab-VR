using UnityEngine;

public class NumberSlider : MonoBehaviour
{
    [Header("Reference")]
    public NumberBlock numberBlock;

    [Header("Slider Settings")]
    public float maxOffset = 0.5f;
    public int maxValue = 20;

    private Vector3 startLocalPosition;

    void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (numberBlock == null || numberBlock.incomingCable != null) return;

        Vector3 localPos = transform.localPosition;

        // Solo permitimos movimiento horizontal
        localPos.y = startLocalPosition.y;
        localPos.z = startLocalPosition.z;

        // Limitamos el recorrido izquierda-derecha
        localPos.x = Mathf.Clamp(
            localPos.x,
            startLocalPosition.x - maxOffset,
            startLocalPosition.x + maxOffset
        );

        transform.localPosition = localPos;

        // Convertimos posición en valor
        float delta = startLocalPosition.x - localPos.x;
        float normalized = delta / maxOffset; // entre -1 y 1
        int value = Mathf.RoundToInt(normalized * maxValue);

        numberBlock.SetValueFromSlider(value);
    }
}
