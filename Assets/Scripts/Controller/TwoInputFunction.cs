using System.Collections.Generic;
using UnityEngine;

public class TwoInputFunction : MonoBehaviour, INodeOutput, INodeInput
{

    [Header("Data Input")]
    public DataCable firstInput;
    public DataCable secondInput;

    [Header("View")]
    public FunctionBlockView view;

    private int actual = 0;
    private List<TwoInputOperation> operations;

    void Awake()
    {
        operations = new List<TwoInputOperation>
        {
            new AddTwoInputOperation(),
            new SubtractTwoInputOperation(),
            new MultiplyTwoInputOperation(),
            new DivideTwoInputOperation(),
            new DotProductOperation(),
            new CrossProductOperation(),
            new MidPointOperation(),
            new AngleOperation()
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

    void Update()
    {
        UpdateTexts();
        UpdateBlockColor();
    }

    TwoInputOperation CurrentOperation
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
        return GetCurrentOutputType() == NodeValueType.Number;
    }

    public bool OutputsVector()
    {
        return GetCurrentOutputType() == NodeValueType.Vector;
    }

    NodeValueType GetCurrentOutputType()
    {
        TwoInputOperation operation = CurrentOperation;

        if (operation == null)
        {
            return NodeValueType.Number;
        }

        NodeValueType firstType = GetCableValueType(firstInput);
        NodeValueType secondType = GetCableValueType(secondInput);

        return operation.GetOutputType(firstType, secondType);
    }

    NodeValueType GetCableValueType(DataCable cable)
    {
        if (cable != null && cable.IsVectorSource())
        {
            return NodeValueType.Vector;
        }

        return NodeValueType.Number;
    }

    NodeValue GetCableValue(DataCable cable)
    {
        if (cable == null)
        {
            return NodeValue.FromNumber(0f);
        }

        if (cable.IsVectorSource())
        {
            return NodeValue.FromVector(cable.GetVectorFromSource());
        }

        return NodeValue.FromNumber(cable.GetValueFromSource());
    }

    public float GetCurrentResult()
    {
        TwoInputOperation operation = CurrentOperation;

        if (operation == null || !OutputsNumber())
        {
            return 0f;
        }

        NodeValue firstValue = GetCableValue(firstInput);
        NodeValue secondValue = GetCableValue(secondInput);
        NodeValue result = operation.Execute(firstValue, secondValue);

        return result.IsNumber ? result.Number : 0f;
    }

    public Vector3 GetCurrentVectorResult()
    {
        TwoInputOperation operation = CurrentOperation;

        if (operation == null || !OutputsVector())
        {
            return Vector3.zero;
        }

        NodeValue firstValue = GetCableValue(firstInput);
        NodeValue secondValue = GetCableValue(secondInput);
        NodeValue result = operation.Execute(firstValue, secondValue);

        return result.IsVector ? result.Vector : Vector3.zero;
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

    void UpdateVisualText()
    {
        if (view == null) return;

        TwoInputOperation operation = CurrentOperation;

        if (operation == null)
        {
            view.SetFunctionLabel("?");
            return;
        }

        view.SetFunctionLabel(operation.Label);
    }

    void UpdateTexts()
    {
        if (view == null) return;

        TwoInputOperation operation = CurrentOperation;

        if (operation == null)
        {
            view.SetInputOutputLabels("?", "?", "?");
            return;
        }

        string outputLabel = OutputsNumber() ? "N" : "V";

        view.SetInputOutputLabels(
            operation.FirstInputLabel,
            operation.SecondInputLabel,
            outputLabel
        );
    }

    void UpdateBlockColor()
    {
        if (view == null) return;

        TwoInputOperation operation = CurrentOperation;

        if (operation != null && operation.UsesNeutralColor)
        {
            view.SetNeutralFunctionColor();
            return;
        }

        view.SetFunctionColor(GetOutputType());
    }

    public bool AcceptsInput(DataCable cable)
    {
        if (cable == null) return false;

        TwoInputOperation operation = CurrentOperation;

        if (operation == null)
        {
            return false;
        }

        NodeValueType inputType = cable.IsVectorSource()
            ? NodeValueType.Vector
            : NodeValueType.Number;

        return operation.AcceptsInput(inputType);
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
        if (cable == null) return false;

        bool isValidPort = port.CompareTag("First InPut") || port.CompareTag("Second InPut");

        if (!isValidPort)
        {
            return false;
        }

        return AcceptsInput(cable);
    }

    public bool IsPortOccupied(DataCable cable, Transform port)
    {
        if (port.CompareTag("First InPut"))
        {
            return firstInput != null && firstInput != cable;
        }

        if (port.CompareTag("Second InPut"))
        {
            return secondInput != null && secondInput != cable;
        }

        return false;
    }

    public void ConnectCable(DataCable cable, Transform port)
    {
        if (port.CompareTag("First InPut"))
        {
            firstInput = cable;
        }
        else if (port.CompareTag("Second InPut"))
        {
            secondInput = cable;
        }
    }

    public void DisconnectCable(DataCable cable, Transform port)
    {
        if (port.CompareTag("First InPut") && firstInput == cable)
        {
            firstInput = null;
        }
        else if (port.CompareTag("Second InPut") && secondInput == cable)
        {
            secondInput = null;
        }
    }
}