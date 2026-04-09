using UnityEngine;

public class DataCable : MonoBehaviour
{
    public GameObject sourceObject;

    private Transform connectedPort;

    public float GetValueFromSource()
    {
        if (sourceObject == null) return 0;

        NumberBlock numberBlock = sourceObject.GetComponent<NumberBlock>();
        if (numberBlock != null)
        {
            return numberBlock.currentValue;
        }

        SelectFunction selectFunction = sourceObject.GetComponent<SelectFunction>();
        if (selectFunction != null)
        {
            return selectFunction.GetCurrentResult();
        }

        return 0;
    }

    public Vector3 GetVectorFromSource()
    {
        if (sourceObject == null) return Vector3.zero;

        VectorBlock vBlock = sourceObject.GetComponent<VectorBlock>();
        if (vBlock != null) return vBlock.currentVector;

        SelectFunction selectFunc = sourceObject.GetComponent<SelectFunction>();
        if (selectFunc != null) return selectFunc.GetCurrentVectorResult();

        return Vector3.zero;
    }

    public void ConnectToPort(Transform port)
    {
        if (port == null) return;

        DisconnectFromPort();
        connectedPort = port;

        SelectFunction bloqueFunc = port.GetComponentInParent<SelectFunction>();
        if (bloqueFunc != null)
        {
            if (port.CompareTag("First InPut"))
            {
                bloqueFunc.firstInput = this;
            }
            else if (port.CompareTag("Second InPut"))
            {
                bloqueFunc.secondInput = this;
            }

            return;
        }

        NumberBlock bloqueNum = port.GetComponentInParent<NumberBlock>();
        if (bloqueNum != null && port.CompareTag("Input"))
        {
            bloqueNum.incomingCable = this;
            return;
        }

        VectorBlock vBlock = port.GetComponentInParent<VectorBlock>();
        if (vBlock != null && port.CompareTag("Input"))
        {
            vBlock.incomingCable = this;
        }
    }

    public void DisconnectFromPort()
    {
        if (connectedPort == null) return;

        SelectFunction bloqueFunc = connectedPort.GetComponentInParent<SelectFunction>();
        if (bloqueFunc != null)
        {
            if (connectedPort.CompareTag("First InPut") && bloqueFunc.firstInput == this)
            {
                bloqueFunc.firstInput = null;
            }
            else if (connectedPort.CompareTag("Second InPut") && bloqueFunc.secondInput == this)
            {
                bloqueFunc.secondInput = null;
            }
        }

        NumberBlock bloqueNum = connectedPort.GetComponentInParent<NumberBlock>();
        if (bloqueNum != null && connectedPort.CompareTag("Input") && bloqueNum.incomingCable == this)
        {
            bloqueNum.incomingCable = null;
        }

        VectorBlock vBlock = connectedPort.GetComponentInParent<VectorBlock>();
        if (vBlock != null && connectedPort.CompareTag("Input") && vBlock.incomingCable == this)
        {
            vBlock.incomingCable = null;
        }

        connectedPort = null;
    }
}
