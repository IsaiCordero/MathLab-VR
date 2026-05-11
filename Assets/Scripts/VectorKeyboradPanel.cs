using UnityEngine;
using TMPro;
using System.Globalization;

public class VectorKeyboardPanel : MonoBehaviour
{
    [Header("References")]
    public VectorBlock targetVectorBlock;
    public TextMeshProUGUI displayText;

    [Header("Current Input")]
    public string selectedAxis = "X";

    private string currentInput = "";

    void Start()
    {
        RefreshDisplay();
    }

    public void HandleButtonPress(string value)
    {
        switch (value)
        {
            case "X":
            case "Y":
            case "Z":
                selectedAxis = value;
                currentInput = "";
                break;

            case "CLEAR":
                currentInput = "";
                break;

            case "CONFIRM":
                ConfirmInput();
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
        if (targetVectorBlock == null) return;

        if (!float.TryParse(currentInput, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedValue))
        {
            return;
        }

        Vector3 v = targetVectorBlock.currentVector;

        switch (selectedAxis)
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

        currentInput = "";
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        if (displayText != null)
        {
            string shownValue = string.IsNullOrEmpty(currentInput) ? "_" : currentInput;
            displayText.text = selectedAxis + ": " + shownValue;
        }
    }
}
