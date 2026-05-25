using UnityEngine;
using TMPro;

public class TwoInputFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText; 
    
    [Header("Settings")]
    public string[] functionKeys = { "SUMA", "RESTA", "MULTIPLICACION", "DIVISION", "PRODUCTO_ESCALAR", "PRODUCTO_VECTORIAL", "PUNTO_MEDIO", "ANGULO" };

    public string[] functionLabels = { "SUMA", "RESTA", "MULTIPLICACIÓN", "DIVISIÓN", "PRODUCTO\nESCALAR", "PRODUCTO\nVECTORIAL", "PUNTO\nMEDIO", "ÁNGULO" };

    [Header("Data Input")]
    public DataCable firstInput;
    public DataCable secondInput;
    
    private int actual = 0;

    void Start()
    {
        if (visualText != null && functionLabels.Length > 0)
        {
            visualText.text = functionLabels[actual];
        }
    }
    public bool OutputsNumber()
    {
        string fn = functionKeys[actual];

        if (fn == "PRODUCTO_ESCALAR" || fn == "ANGULO")
            return true;

        if (fn == "SUMA" || fn == "RESTA" || fn == "MULTIPLICACION" || fn == "DIVISION")
        {
            bool aIsVector = firstInput != null && firstInput.IsVectorSource();
            bool bIsVector = secondInput != null && secondInput.IsVectorSource();

            return !aIsVector && !bIsVector;
        }

        return false;
    }   

    public bool OutputsVector()
    {
        return !OutputsNumber();
    }

    public float GetCurrentResult()
    {
        float valA = (firstInput != null) ? firstInput.GetValueFromSource() : 0f;
        float valB = (secondInput != null) ? secondInput.GetValueFromSource() : 0f;

        bool aIsVector = firstInput != null && firstInput.IsVectorSource();
        bool bIsVector = secondInput != null && secondInput.IsVectorSource();

        Vector3 v1 = (firstInput != null) ? firstInput.GetVectorFromSource() : Vector3.zero;
        Vector3 v2 = (secondInput != null) ? secondInput.GetVectorFromSource() : Vector3.zero;

        float resultado = 0f;

        switch (functionKeys[actual])
        {
            case "SUMA":
                if (!aIsVector && !bIsVector)
                {
                    resultado = valA + valB;
                }
                break;

            case "RESTA":
                if (!aIsVector && !bIsVector)
                {
                    resultado = valA - valB;
                }
                break;

            case "MULTIPLICACION":
                if (!aIsVector && !bIsVector)
                {
                    resultado = valA * valB;
                }
                break;

            case "DIVISION":
                if (!aIsVector && !bIsVector)
                {
                    resultado = (valB != 0f) ? valA / valB : 0f;
                }
                break;

            case "PRODUCTO_ESCALAR":
                if (firstInput != null && secondInput != null)
                {
                    resultado = Vector3.Dot(v1, v2);
                }
                break;

            case "ANGULO":
                if (firstInput != null && secondInput != null)
                {
                    resultado = Vector3.Angle(v1, v2);
                }
                break;

            default:
                resultado = 0f;
                break;
        }

        return resultado;
    }

    public Vector3 GetCurrentVectorResult()
    {
        if (firstInput == null || secondInput == null) return Vector3.zero;

        bool aIsVector = firstInput.IsVectorSource();
        bool bIsVector = secondInput.IsVectorSource();

        float valA = firstInput.GetValueFromSource();
        float valB = secondInput.GetValueFromSource();

        Vector3 v1 = firstInput.GetVectorFromSource();
        Vector3 v2 = secondInput.GetVectorFromSource();

        Vector3 resultado = Vector3.zero;

        switch (functionKeys[actual])
        {
            case "SUMA":
                if (aIsVector && bIsVector)
                {
                    resultado = v1 + v2;
                }
                else if (aIsVector && !bIsVector)
                {
                    resultado = v1 + new Vector3(valB, valB, valB);
                }
                else if (!aIsVector && bIsVector)
                {
                    resultado = new Vector3(valA, valA, valA) + v2;
                }
                break;

            case "RESTA":
                if (aIsVector && bIsVector)
                {
                    resultado = v1 - v2;
                }
                else if (aIsVector && !bIsVector)
                {
                    resultado = v1 - new Vector3(valB, valB, valB);
                }
                else if (!aIsVector && bIsVector)
                {
                    resultado = new Vector3(valA, valA, valA) - v2;
                }
                break;

            case "MULTIPLICACION":
                if (aIsVector && bIsVector)
                {
                    resultado = new Vector3(
                        v1.x * v2.x,
                        v1.y * v2.y,
                        v1.z * v2.z
                    );
                }
                else if (aIsVector && !bIsVector)
                {
                    resultado = v1 * valB;
                }
                else if (!aIsVector && bIsVector)
                {
                    resultado = v2 * valA;
                }
                break;

            case "DIVISION":
                if (aIsVector && bIsVector)
                {
                    resultado = new Vector3(
                        v2.x != 0f ? v1.x / v2.x : 0f,
                        v2.y != 0f ? v1.y / v2.y : 0f,
                        v2.z != 0f ? v1.z / v2.z : 0f
                    );
                }
                else if (aIsVector && !bIsVector)
                {
                    resultado = (valB != 0f) ? v1 / valB : Vector3.zero;
                }
                else if (!aIsVector && bIsVector)
                {
                    resultado = new Vector3(
                        v2.x != 0f ? valA / v2.x : 0f,
                        v2.y != 0f ? valA / v2.y : 0f,
                        v2.z != 0f ? valA / v2.z : 0f
                    );
                }
                break;

            case "PRODUCTO_VECTORIAL":
                resultado = Vector3.Cross(v1, v2);
                break;

            case "PUNTO_MEDIO":
                resultado = (v1 + v2) / 2f;
                break;

            default:
                resultado = Vector3.zero;
                break;
        }

        return resultado;
    }

    public void SetFunctionByIndex(int index)
    {
        if (functionKeys == null || functionKeys.Length == 0) return;

        if (index < 0 || index >= functionKeys.Length)
        {
            index = 0;
        }

        actual = index;

        if (visualText != null)
        {
            visualText.text = functionLabels[actual];
        }
    }

}