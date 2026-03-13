using UnityEngine;
using TMPro;

public class SelectFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText; 
    
    [Header("Settings")]
    public string[] functions = { "SUMA", "RESTA", "MULTIPLICACIÓN", "DIVISIÓN" };
    
    private int actual = 0;

    void Start()
    {
        if (visualText != null && functions.Length > 0)
        {
            visualText.text = functions[actual];
        }
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