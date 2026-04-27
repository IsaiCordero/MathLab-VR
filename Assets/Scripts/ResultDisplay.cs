using UnityEngine;
using TMPro;
public class ResultDisplay : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TwoInputFunction functionBlock;

    void Update()
    {
        if(functionBlock != null && resultText != null)
        {
            float value = functionBlock.GetCurrentResult();

            resultText.text = value.ToString("F1");
        }
    }
}
