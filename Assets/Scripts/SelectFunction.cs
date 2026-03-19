using UnityEngine;
using TMPro;

public class SelectFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText; 
    
    [Header("Settings")]
    public string[] functions = { "SUMA", "RESTA", "MULTIPLICACIÓN", "DIVISIÓN" };

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
            default: resultado = 0; break;
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
}