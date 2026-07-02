using UnityEngine;

public class VectorBlock : MonoBehaviour, INodeOutput, INodeInput
{
    [Header("Vector Data")]
    public Vector3 currentVector;

    [Header("Vector Area")]
    public Transform vectorCube;
    public Transform centerReference;

    [Header("Arrow View")]
    public VectorArrowView arrowView;

    [Header("Settings")]
    public float lerpSpeed = 10f;
    public float vectorScaleFactor = 10f;

    [Header("Cable")]
    public DataCable incomingCable;

    [Header("Keyboard")]
    public VectorKeyboardController keyboardController;

    [Header("Input Mode")]
    public BlockInputModeController inputModeController;
    private bool lastHadIncomingCable;

    [Header("View")]
    public VectorBlockView view;

    private bool wasConnected = false;

    private Vector3 lastDisplayedVector;
    private Vector3 lastCubeLocalPosition;

    private bool usingKeyboardValue = false;
    private Vector3 keyboardExactVector = Vector3.zero;
    private Vector3 keyboardVisualPosition;


    void Start()
    {
        if (arrowView == null)
        {
            arrowView = GetComponent<VectorArrowView>();
        }

        if (view == null)
        {
            view = GetComponent<VectorBlockView>();
        }

        UpdateVisuals();
        lastDisplayedVector = currentVector;

        if (inputModeController == null)
        {
            inputModeController = GetComponent<BlockInputModeController>();
        }

        lastHadIncomingCable = incomingCable != null;
        
        if (inputModeController != null)
        {
            inputModeController.SetEditable(!lastHadIncomingCable);
        }

        if (keyboardController == null)
        {
            keyboardController = GetComponent<VectorKeyboardController>();
        }

        if (keyboardController != null)
        {
            keyboardController.Initialize(this);
        }
    }

    void Update()
    {
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

        if (vectorCube == null || centerReference == null)
            return;

        if (incomingCable != null)
        {
            wasConnected = true;

            currentVector = incomingCable.GetVectorFromSource();

            Vector3 targetPosition = VectorPositionMapper.VectorToLocalPosition(
                currentVector,
                centerReference.localPosition,
                vectorScaleFactor
            );

            if ((vectorCube.localPosition - targetPosition).sqrMagnitude > 0.000001f)
            {
                vectorCube.localPosition = Vector3.Lerp(
                    vectorCube.localPosition,
                    targetPosition,
                    Time.deltaTime * lerpSpeed
                );
            }
            else
            {
                vectorCube.localPosition = targetPosition;
            }
        }
        else
        {
            if (wasConnected)
            {
                ResetBlock();
            }

            if (usingKeyboardValue)
            {
                if ((vectorCube.localPosition - keyboardVisualPosition).sqrMagnitude > 0.000001f)
                {
                    usingKeyboardValue = false;
                }
                else
                {
                    currentVector = keyboardExactVector;
                }
            }

            if (!usingKeyboardValue)
            {
                currentVector = VectorPositionMapper.LocalPositionToVector(
                    vectorCube.localPosition,
                    centerReference.localPosition,
                    vectorScaleFactor
                );
            }
        }

        if (currentVector != lastDisplayedVector)
        {
            UpdateVisuals();
            lastDisplayedVector = currentVector;
        }
    }

    void LateUpdate()
    {
        if (vectorCube == null || centerReference == null)
            return;

        if (vectorCube.localPosition != lastCubeLocalPosition)
        {
            UpdateDynamicArrow();
            lastCubeLocalPosition = vectorCube.localPosition;
        }
        else if (arrowView != null && arrowView.ShouldRefreshWhenHidden())
        {
            UpdateDynamicArrow();
        }
    }

    void ResetBlock()
    {
        if (vectorCube != null && centerReference != null)
        {
            vectorCube.localPosition = centerReference.localPosition;
            lastCubeLocalPosition = vectorCube.localPosition;
        }

        currentVector = Vector3.zero;
        wasConnected = false;
        usingKeyboardValue = false;
        keyboardExactVector = Vector3.zero;
        keyboardVisualPosition = centerReference.localPosition;


        if (arrowView != null)
        {
            arrowView.Hide();
        }
    }

    public void SetValueFromKeyboard(Vector3 newVector)
    {
        keyboardExactVector = newVector;
        currentVector = newVector;
        usingKeyboardValue = true;

        if (vectorCube != null && centerReference != null)
        {
            Vector3 targetPosition = VectorPositionMapper.VectorToLocalPosition(
                currentVector,
                centerReference.localPosition,
                vectorScaleFactor
            );
            VectorController controller = vectorCube.GetComponent<VectorController>();
            if (controller != null)
            {
                targetPosition.x = Mathf.Clamp(targetPosition.x, controller.minLocalPosition.x, controller.maxLocalPosition.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, controller.minLocalPosition.y, controller.maxLocalPosition.y);
                targetPosition.z = Mathf.Clamp(targetPosition.z, controller.minLocalPosition.z, controller.maxLocalPosition.z);
            }

            vectorCube.localPosition = targetPosition;
            keyboardVisualPosition = targetPosition;
        }

        UpdateVisuals();
    }
    void UpdateVisuals()
    {
        if (view != null)
        {
            view.UpdateValue(currentVector);
        }
    }

    public void OpenKeyboard()
    {
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
    }

    void UpdateDynamicArrow()
    {
        if (arrowView == null || vectorCube == null || centerReference == null)
        {
            return;
        }

        Vector3 localOffset = vectorCube.localPosition - centerReference.localPosition;
        arrowView.UpdateArrow(localOffset, centerReference.localPosition);
    }
    
    private void OnDestroy()
    {
        CloseKeyboard();
    }

    public NodeValue GetOutputValue()
    {
        return NodeValue.FromVector(currentVector);
    }

    public NodeValueType GetOutputType()
    {
        return NodeValueType.Vector;
    }

    public bool AcceptsCable(DataCable cable, Transform port)
    {
        return port.CompareTag("Input") && cable != null && cable.IsVectorSource();
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
