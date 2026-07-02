using UnityEngine;

public class DataCable : MonoBehaviour
{
    public GameObject sourceObject;

    private Transform connectedPort;

    public float GetValueFromSource()
    {
        NodeValue outputValue = GetOutputValue();

        if (!outputValue.IsNumber)
        {
            return 0f;
        }

        return outputValue.Number;
    }

    public Vector3 GetVectorFromSource()
    {
        NodeValue outputValue = GetOutputValue();

        if (!outputValue.IsVector)
        {
            return Vector3.zero;
        }

        return outputValue.Vector;
    }

    public bool ConnectToPort(Transform port)
    {
        if (port == null) return false;

        INodeInput nodeInput = GetNodeInput(port);

        if (nodeInput == null)
        {
            return false;
        }

        if (nodeInput.IsPortOccupied(this, port))
        {
            return false;
        }

        InputPortVisual portVisual = port.GetComponent<InputPortVisual>();

        if (!nodeInput.AcceptsCable(this, port))
        {
            if (portVisual != null)
            {
                portVisual.SetInvalid();
            }

            return false;
        }

        DisconnectFromPort();

        connectedPort = port;
        nodeInput.ConnectCable(this, port);

        if (portVisual != null)
        {
            portVisual.SetConnected(true);
        }

        return true;
    }

    public void DisconnectFromPort()
    {
        if (connectedPort == null) return;

        InputPortVisual portVisual = connectedPort.GetComponent<InputPortVisual>();
        if (portVisual != null)
        {
            portVisual.SetConnected(false);
        }

        INodeInput nodeInput = GetNodeInput(connectedPort);
        if (nodeInput != null)
        {
            nodeInput.DisconnectCable(this, connectedPort);
        }

        connectedPort = null;
    }

    public bool IsNumberSource()
    {
        INodeOutput sourceOutput = GetSourceOutput();

        if (sourceOutput == null)
        {
            return false;
        }

        return sourceOutput.GetOutputType() == NodeValueType.Number;
    }

    public bool IsVectorSource()
    {
        INodeOutput sourceOutput = GetSourceOutput();

        if (sourceOutput == null)
        {
            return false;
        }

        return sourceOutput.GetOutputType() == NodeValueType.Vector;
    }

    public NodeValue GetOutputValue()
    {
        INodeOutput sourceOutput = GetSourceOutput();

        if (sourceOutput == null)
        {
            return NodeValue.FromNumber(0f);
        }

        return sourceOutput.GetOutputValue();
    }

    INodeOutput GetSourceOutput()
    {
        if (sourceObject == null) return null;

        return sourceObject.GetComponent<INodeOutput>();
    }

    INodeInput GetNodeInput(Transform port)
    {
        if (port == null) return null;

        return port.GetComponentInParent<INodeInput>();
    }
}