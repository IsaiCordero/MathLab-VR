using UnityEngine;

public class OpenKeyboardButton : FixedGrabbableButton
{
    [Header("References")]
    public VectorBlock targetVectorBlock;
    public NumberBlock targetNumberBlock;

    protected override void OnButtonPressed()
    {
        if (targetVectorBlock != null)
        {
            targetVectorBlock.OpenKeyboard();
        }
        else if (targetNumberBlock != null)
        {
            targetNumberBlock.OpenKeyboard();
        }
    }
}