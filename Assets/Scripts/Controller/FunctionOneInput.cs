using System.Collections.Generic;
using UnityEngine;

public class FunctionOneInput : MonoBehaviour, INodeOutput, INodeInput
{
    [Header("View")]
    public FunctionBlockView view;

    [Header("Data Input")]
    public DataCable input;

    private int actual = 0;
    private List<OneInputVectorOperation> operations;

    void Awake()
    {
        operations = new List<OneInputVectorOperation>
        {
            new MagnitudeOperation(),
            new NormalizeOperation(),
            new OppositeOperation(),
            new AbsoluteOperation()
        };
    }

    void Start()
    {
        if (view == null)
        {
            view = GetComponent<FunctionBlockView>();
        }

        UpdateVisualText();
        UpdateBlockColor();
        UpdateTexts();
    }

    OneInputVectorOperation CurrentOperation
    {
        get
        {
            if (operations == null || operations.Count == 0)
            {
                return null;
            }

            actual = Mathf.Clamp(actual, 0, operations.Count - 1);
            return operations[actual];
        }
    }

    public bool OutputsNumber()
    {
        OneInputVectorOperation operation = CurrentOperation;
        return operation != null && operation.OutputType == OperationOutputType.Number;
    }

    public bool OutputsVector()
    {
        OneInputVectorOperation operation = CurrentOperation;
        return operation != null && operation.OutputType == OperationOutputType.Vector;
    }

    void UpdateTexts()
    {
        if (view == null) return;

        OneInputVectorOperation operation = CurrentOperation;

        if (operation == null)
        {
            view.SetSingleOutputLabel("?");
            return;
        }

        view.SetSingleOutputLabel(operation.OutputLabel);
    }

    void UpdateVisualText()
    {
        if (view == null) return;

        OneInputVectorOperation operation = CurrentOperation;

        if (operation == null)
        {
            view.SetFunctionLabel("?");
            return;
        }

        view.SetFunctionLabel(operation.Label);
    }

    public float GetCurrentResult()
    {
        if (input == null) return 0f;

        OneInputVectorOperation operation = CurrentOperation;
        if (operation == null || operation.OutputType != OperationOutputType.Number)
        {
            return 0f;
        }

        Vector3 v = input.GetVectorFromSource();
        return operation.ExecuteNumber(v);
    }

    public Vector3 GetCurrentVectorResult()
    {
        if (input == null) return Vector3.zero;

        OneInputVectorOperation operation = CurrentOperation;
        if (operation == null || operation.OutputType != OperationOutputType.Vector)
        {
            return Vector3.zero;
        }

        Vector3 v = input.GetVectorFromSource();
        return operation.ExecuteVector(v);
    }

    public void SetFunctionByIndex(int index)
    {
        if (operations == null || operations.Count == 0)
        {
            Awake();
        }

        if (operations == null || operations.Count == 0) return;

        actual = Mathf.Clamp(index, 0, operations.Count - 1);

        UpdateVisualText();
        UpdateBlockColor();
        UpdateTexts();
    }

    void UpdateBlockColor()
    {
        if (view == null) return;

        view.SetFunctionColor(GetOutputType());
    }

    public NodeValue GetOutputValue()
    {
        if (OutputsNumber())
        {
            return NodeValue.FromNumber(GetCurrentResult());
        }

        return NodeValue.FromVector(GetCurrentVectorResult());
    }

    public NodeValueType GetOutputType()
    {
        return OutputsNumber() ? NodeValueType.Number : NodeValueType.Vector;
    }

    public bool AcceptsCable(DataCable cable, Transform port)
    {
        return port.CompareTag("Input") && cable != null && cable.IsVectorSource();
    }

    public bool IsPortOccupied(DataCable cable, Transform port)
    {
        return port.CompareTag("Input") && input != null && input != cable;
    }

    public void ConnectCable(DataCable cable, Transform port)
    {
        if (!port.CompareTag("Input")) return;

        input = cable;
    }

    public void DisconnectCable(DataCable cable, Transform port)
    {
        if (!port.CompareTag("Input")) return;

        if (input == cable)
        {
            input = null;
        }
    }
}