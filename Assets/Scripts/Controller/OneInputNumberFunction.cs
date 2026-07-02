using System.Collections.Generic;
using UnityEngine;

public class OneInputNumberFunction : MonoBehaviour, INodeOutput, INodeInput, IFunctionBlock
{
    [Header("Data Input")]
    public DataCable input;

    [Header("View")]
    public FunctionBlockView view;

    private int actual = 0;
    private List<OneInputNumberOperation> operations;

    void Awake()
    {
        InitializeOperations();
    }

    void InitializeOperations()
    {
        operations = new List<OneInputNumberOperation>
        {
            new SinOperation(),
            new CosOperation(),
            new TanOperation(),
            new AsinOperation(),
            new AcosOperation(),
            new AtanOperation()
        };
    }

    void Start()
    {
        if (view == null)
        {
            view = GetComponent<FunctionBlockView>();
        }

        UpdateVisualText();
    }

    OneInputNumberOperation CurrentOperation
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
        return true;
    }

    public float GetCurrentResult()
    {
        if (input == null) return 0f;

        OneInputNumberOperation operation = CurrentOperation;
        if (operation == null)
        {
            return 0f;
        }

        float value = input.GetValueFromSource();
        return operation.Execute(value);
    }

    public void SetFunctionByIndex(int index)
    {
        if (operations == null || operations.Count == 0)
        {
            InitializeOperations();
        }

        if (operations == null || operations.Count == 0) return;

        actual = Mathf.Clamp(index, 0, operations.Count - 1);
        UpdateVisualText();
    }

    void UpdateVisualText()
    {
        if (view == null) return;

        OneInputNumberOperation operation = CurrentOperation;

        if (operation == null)
        {
            view.SetFunctionLabel("?");
            return;
        }

        view.SetFunctionLabel(operation.Label);
        view.SetFunctionColor(NodeValueType.Number);
    }

    public NodeValue GetOutputValue()
    {
        return NodeValue.FromNumber(GetCurrentResult());
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