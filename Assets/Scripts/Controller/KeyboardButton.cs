using UnityEngine;

public class KeyboardButton : FixedGrabbableButton
{
    [Header("References")]
    public VectorKeyboardPanel keyboardPanel;
    public NumberKeyboardPanel numberKeyboardPanel;

    [Header("Button Action")]
    public string buttonValue;

    protected override void OnButtonPressed()
    {
        if (keyboardPanel != null)
        {
            keyboardPanel.HandleButtonPress(buttonValue);
        }
        else if (numberKeyboardPanel != null)
        {
            numberKeyboardPanel.HandleButtonPress(buttonValue);
        }
    }
}