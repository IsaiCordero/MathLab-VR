using UnityEngine;
using TMPro;
using System.Globalization;

public class NumberKeyboardPanel : MonoBehaviour
{
    [Header("References")]
    public NumberBlock targetNumberBlock;
    public TextMeshProUGUI displayText;

    private string currentInput = "";

    void Start()
    {
        RefreshDisplay();
    }

    public void HandleButtonPress(string value)
    {
        switch (value)
        {
            case "CLEAR":
                currentInput = "";
                break;

            case "CONFIRM":
                ConfirmInput();
                return;

            case "CANCEL":
                CancelInput();
                return;

            case "-":
                if (!currentInput.Contains("-") && currentInput.Length == 0)
                {
                    currentInput += "-";
                }
                break;

            case ".":
                if (!currentInput.Contains("."))
                {
                    if (string.IsNullOrEmpty(currentInput) || currentInput == "-")
                    {
                        currentInput += "0.";
                    }
                    else
                    {
                        currentInput += ".";
                    }
                }
                break;

            default:
                currentInput += value;
                break;
        }

        RefreshDisplay();
    }

    void ConfirmInput()
    {
        if (targetNumberBlock == null) return;

        if (!float.TryParse(currentInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue))
        {
            return;
        }

        targetNumberBlock.SetValueFromKeyboard(parsedValue);
        targetNumberBlock.CloseKeyboard();
    }

    void CancelInput()
    {
        if (targetNumberBlock != null)
        {
            targetNumberBlock.CloseKeyboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void RefreshDisplay()
    {
        if (displayText != null)
        {
            displayText.text = string.IsNullOrEmpty(currentInput) ? "_" : currentInput;
        }
    }
}