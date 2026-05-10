using UnityEngine;
using TMPro;

public class TwoInputFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText; 
    
    [Header("Settings")]
    public string[] functions = { "SUMA", "RESTA", "MULTIPLICACIÓN", "DIVISIÓN", "PRODUCTO ESCALAR", "PRODUCTO VECTORIAL", "PUNTO MEDIO", "ÁNGULO" };

    [Header("Data Input")]
    public DataCable firstInput;
    public DataCable secondInput;
    
    private int actual = 0;

    void Start()
    {
        if (visualText != null && functions.Length > 0)
        {
            visualText.text = functions[actual];
        }
    }
    public bool OutputsNumber()
    {
        string fn = functions[actual];
        return fn == "PRODUCTO ESCALAR" || fn == "ÁNGULO";
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

        switch (functions[actual])
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

            case "MULTIPLICACIÓN":
                if (!aIsVector && !bIsVector)
                {
                    resultado = valA * valB;
                }
                break;

            case "DIVISIÓN":
                if (!aIsVector && !bIsVector)
                {
                    resultado = (valB != 0f) ? valA / valB : 0f;
                }
                break;

            case "PRODUCTO ESCALAR":
                if (firstInput != null && secondInput != null)
                {
                    resultado = Vector3.Dot(v1, v2);
                }
                break;

            case "ÁNGULO":
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

        switch (functions[actual])
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

            case "MULTIPLICACIÓN":
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

            case "DIVISIÓN":
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

            case "PRODUCTO VECTORIAL":
                resultado = Vector3.Cross(v1, v2);
                break;

            case "PUNTO MEDIO":
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
        if (functions == null || functions.Length == 0) return;

        if (index < 0 || index >= functions.Length)
        {
            index = 0;
        }

        actual = index;

        if (visualText != null)
        {
            visualText.text = functions[actual];
        }
    }

}