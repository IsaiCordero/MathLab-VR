using UnityEngine;
using TMPro;

public class TwoInputFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText; 
    
    [Header("Settings")]
    public string[] functions = { "SUMA", "RESTA", "MULTIPLICACIÓN", "DIVISIÓN", "PRODUCTO ESCALAR", "PRODUCTO VECTORIAL" };

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

    public float GetCurrentResult()
    {
        float valA = (firstInput != null) ?firstInput.GetValueFromSource() : 0;
        float valB = (secondInput != null) ?secondInput.GetValueFromSource() : 0;
        float resultado = 0;
        switch (functions[actual])
        {
            case "SUMA": resultado = valA + valB; break;
            case "RESTA": resultado = valA - valB; break;
            case "MULTIPLICACIÓN": resultado = valA * valB; break;
            case "DIVISIÓN": resultado = (valB != 0) ? (float)valA / valB : 0; break;
            case "PRODUCTO ESCALAR":
                if (firstInput != null && secondInput != null)
                {
                    Vector3 v1 = firstInput.GetVectorFromSource();
                    Vector3 v2 = secondInput.GetVectorFromSource();
                    resultado = Vector3.Dot(v1, v2);
                }
                else
                {
                    resultado = 0;
                }
                break;

            default: resultado = 0; break;

            case "PRODUCTO VECTORIAL":
                resultado = 0;
                break;
        }
        return resultado;
    }
    public Vector3 GetCurrentVectorResult()
    {
        if (firstInput == null || secondInput == null) return Vector3.zero;

        Vector3 v1 = firstInput.GetVectorFromSource();
        Vector3 v2 = secondInput.GetVectorFromSource();

        Vector3 resultado = v1;
        switch (functions[actual])
        {
            case "SUMA": resultado = v1 + v2; break;
            case "RESTA": resultado = v1 - v2; break;
            case "MULTIPLICACIÓN": resultado = new Vector3(
                v1.x * v2.x,
                v1.y * v2.y,
                v1.z * v2.z
            ); break;
            case "DIVISIÓN": resultado = new Vector3(
                v2.x != 0 ? v1.x / v2.x : 0,
                v2.y != 0 ? v1.y / v2.y : 0,
                v2.z != 0 ? v1.z / v2.z : 0
            ); break;
            case "PRODUCTO ESCALAR":
                resultado = Vector3.zero;
                break;
            case "PRODUCTO VECTORIAL":
                resultado = Vector3.Cross(v1, v2);
                break;
        }
        return resultado;
    }
    public void ChangeNextFunction()
    {
        actual++;
        
        if (actual >= functions.Length) 
        {
            actual = 0;
        }

        if (visualText != null)
        {
            visualText.text = functions[actual];
        }
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