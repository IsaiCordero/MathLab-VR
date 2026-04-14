using UnityEngine;
using TMPro;

public class VectorBlock : MonoBehaviour
{
    [Header("Text UI")]
    public TextMeshProUGUI valueText;

    [Header("Vector Data")]
    public Vector3 currentVector;

    [Header("Vector Area")]
    public Transform vectorCube;
    public Transform centerReference;

    [Header("Settings")]
    public float lerpSpeed = 10f;

    [Header("Cable")]
    public DataCable incomingCable;

    private bool wasConnected = false;

    void Start()
    {
        UpdateVisuals();
    }

    void Update()
    {
        if (vectorCube == null || centerReference == null)
            return;

        if (incomingCable != null)
        {
            wasConnected = true;

            // 1. Leemos el vector que llega por el cable
            currentVector = incomingCable.GetVectorFromSource();

            // 2. Movemos el cubo dentro del área local hacia ese vector
            vectorCube.localPosition = Vector3.Lerp(
                vectorCube.localPosition,
                centerReference.localPosition + currentVector,
                Time.deltaTime * lerpSpeed
            );
        }
        else
        {
            // Si antes estaba conectado y ahora no, reiniciamos
            if (wasConnected)
            {
                ResetBlock();
            }

            // En modo manual, el vector sale de la posición local del cubo
            Vector3 localOffset = vectorCube.localPosition - centerReference.localPosition;
            localOffset.x = -localOffset.x;
            currentVector = localOffset * 10f;

        }

        UpdateVisuals();
    }

    void ResetBlock()
    {
        if (vectorCube != null && centerReference != null)
        {
            vectorCube.localPosition = centerReference.localPosition;
        }

        currentVector = Vector3.zero;
        wasConnected = false;
    }

    void UpdateVisuals()
    {
        if (valueText != null)
        {
            string x = currentVector.x.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            string y = currentVector.y.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            string z = currentVector.z.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);

            valueText.text = $"({x}, {y}, {z})";
        }
    }
}
