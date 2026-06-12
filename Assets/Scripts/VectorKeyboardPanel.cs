using UnityEngine;
using TMPro;
using System.Globalization;

public class VectorKeyboardPanel : MonoBehaviour 
{
    [Header("References")]
    public VectorBlock targetVectorBlock;
    public TextMeshProUGUI displayText;

    [Header("Current Input")]
    public string selectAxis = "X";
    public string currentInput = "";

    private void Start() {
        RefreshDisplay();
    }

    public void HandleButtonPress(string value)
    {
        switch (value)
        {
            case "X":
            case "Y":
            case "Z":
                selectAxis = value;
                currentInput = "";
                break;
            
            case "CLEAR":
                currentInput = "";
                break;

            case "CONFIRM":
                ConfirmInput();
                break;

            case "-":
                if(!currentInput.Contains("-") && currentInput.Length == 0)
                {
                    currentInput += "-";
                }
                break;
            
            case ".":
                if (!currentInput.Contains("."))
                {
                    if(string.IsNullOrEmpty(currentInput) || currentInput == "-")
                    {
                        currentInput += "0.";
                    }
                    else
                    {
                        currentInput += ".";
                    }
                }
                break;
            
            case "CANCEL":
                CancelInput();
                return;

            default:
                currentInput += value;
                break;
        }

        RefreshDisplay();
    }

    void ConfirmInput()
    {
        if(targetVectorBlock == null) return;

        if(!float.TryParse(currentInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue))
        {
            return;
        }

        Vector3 v = targetVectorBlock.currentVector;

        switch (selectAxis)
        {
            case "X":
                v.x = parsedValue;
                break;
            
            case "Y":
                v.y = parsedValue;
                break;
            
            case "Z":
                v.z = parsedValue;
                break;
        }

        targetVectorBlock.SetVectorManually(v);
        targetVectorBlock.CloseKeyboard();
    }

    void CancelInput()
    {
        if(targetVectorBlock != null)
        {
            targetVectorBlock.CloseKeyboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void RefreshDisplay()
    {
        if(displayText != null)
        {
            string showValue;
            if (string.IsNullOrEmpty(currentInput))
            {
                showValue = "_";
            }
            else
            {
                showValue = currentInput;
            }
            displayText.text = selectAxis + ": " + showValue;
        }
    }
}