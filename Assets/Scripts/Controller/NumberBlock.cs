using UnityEngine;

public class NumberBlock : MonoBehaviour, INodeOutput, INodeInput
{

    [Header("Settings")]
    public float currentValue = 0;

    [Header("Cable")]
    public DataCable incomingCable;

    [Header("Input Mode")]
    public BlockInputModeController inputModeController;
    private bool lastHadIncomingCable;

    [Header("View")]
    public NumberBlockView view;

    [Header("Slider")]
    public NumberSliderController sliderController;

    [Header("Keyboard")]
    public NumberKeyboardController keyboardController;


    void Start()
    {
        if (view == null)
        {
            view = GetComponent<NumberBlockView>();
        }
        UpdateVisuals();

        
        if (inputModeController == null)
        {
            inputModeController = GetComponent<BlockInputModeController>();
        }

        lastHadIncomingCable = incomingCable != null;
        
        if (inputModeController != null)
        {
            inputModeController.SetEditable(!lastHadIncomingCable);
        }

        if (sliderController == null)
        {
            sliderController = GetComponent<NumberSliderController>();
        }

        if (sliderController != null)
        {
            sliderController.Initialize();
        }

        if (keyboardController == null)
        {
            keyboardController = GetComponent<NumberKeyboardController>();
        }

        if (keyboardController != null)
        {
            keyboardController.Initialize(this);
        }
    }

    void Update()
    {
        if(incomingCable != null)
        {
            float result = incomingCable.GetValueFromSource();
            currentValue = result;
            if (view != null)
            {
                view.UpdateValue(result);
            }
        }
        else if (sliderController != null && sliderController.TryReadValue(out float sliderValue))
        {
            currentValue = sliderValue;
            UpdateVisuals();
        }

        bool hasIncomingCable = incomingCable != null;

        if (hasIncomingCable != lastHadIncomingCable)
        {
            if (inputModeController != null)
            {
                inputModeController.SetEditable(!hasIncomingCable);
            }

            if (hasIncomingCable)
            {
                CloseKeyboard();
            }

            lastHadIncomingCable = hasIncomingCable;
        }
    }

    void UpdateVisuals()
    {
        if (view != null && incomingCable == null)
        {
            view.UpdateValue(currentValue);
        }
    }

    public void SetValueFromSlider(int value)
    {
        if (incomingCable != null) return;

            currentValue = value;
            UpdateVisuals();
    }

    public void SetValueFromKeyboard(float value)
    {
        if (incomingCable != null) return;

        currentValue = value;

        if (sliderController != null)
        {
            sliderController.SetValue(currentValue);
        }

        UpdateVisuals();
    }

    public void OpenKeyboard()
    {
        if (sliderController != null)
        {
            sliderController.SetVisible(false);
        }

        if (keyboardController != null)
        {
            keyboardController.Open();
        }
    }

    public void CloseKeyboard()
    {
        if (keyboardController != null)
        {
            keyboardController.Close();
        }

        if (sliderController != null)
        {
            sliderController.SetVisible(true);
        }
    }

    private void OnDestroy()
    {
        CloseKeyboard();
    }

    public NodeValue GetOutputValue()
    {
        return NodeValue.FromNumber(currentValue);
    }

    public NodeValueType GetOutputType()
    {
        return NodeValueType.Number;
    }

    public bool AcceptsCable(DataCable cable, Transform port)
    {
        return port.CompareTag("Input") && cable != null && cable.IsNumberSource();
    }

    public bool IsPortOccupied(DataCable cable, Transform port)
    {
        return port.CompareTag("Input") && incomingCable != null && incomingCable != cable;
    }

    public void ConnectCable(DataCable cable, Transform port)
    {
        if (!port.CompareTag("Input")) return;

        incomingCable = cable;
    }

    public void DisconnectCable(DataCable cable, Transform port)
    {
        if (!port.CompareTag("Input")) return;

        if (incomingCable == cable)
        {
            incomingCable = null;
        }
    }
}
