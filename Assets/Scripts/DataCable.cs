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

        TwoInputFunction functionTwoInput = sourceObject.GetComponent<TwoInputFunction>();
        if (functionTwoInput != null)
        {
            return functionTwoInput.GetCurrentResult();
        }

        FunctionOneInput oneInputFunction = sourceObject.GetComponent<FunctionOneInput>();
        if(oneInputFunction != null)
        {
            return oneInputFunction.GetCurrentResult();
        }

        OneInputNumberFunction oneInputNumberFunction = sourceObject.GetComponent<OneInputNumberFunction>();
        if (oneInputNumberFunction != null)
        {
            return oneInputNumberFunction.GetCurrentResult();
        }
        return 0;
    }

    public Vector3 GetVectorFromSource()
    {
        if (sourceObject == null) return Vector3.zero;

        VectorBlock vBlock = sourceObject.GetComponent<VectorBlock>();
        if (vBlock != null) return vBlock.currentVector;

        TwoInputFunction functionTwoInput = sourceObject.GetComponent<TwoInputFunction>();
        if (functionTwoInput != null) return functionTwoInput.GetCurrentVectorResult();

        FunctionOneInput oneInputFunction = sourceObject.GetComponent<FunctionOneInput>();
        if(oneInputFunction != null) return oneInputFunction.GetCurrentVectorResult();

        return Vector3.zero;
    }
    private bool PortAlreadyOccupied(Transform port)
    {
        TwoInputFunction bloqueFunc = port.GetComponentInParent<TwoInputFunction>();
        if (bloqueFunc != null)
        {
            if (port.CompareTag("First InPut"))
                return bloqueFunc.firstInput != null && bloqueFunc.firstInput != this;

            if (port.CompareTag("Second InPut"))
                return bloqueFunc.secondInput != null && bloqueFunc.secondInput != this;
        }

        NumberBlock bloqueNum = port.GetComponentInParent<NumberBlock>();
        if (bloqueNum != null && port.CompareTag("Input"))
        {
            return bloqueNum.incomingCable != null && bloqueNum.incomingCable != this;
        }

        VectorBlock vBlock = port.GetComponentInParent<VectorBlock>();
        if (vBlock != null && port.CompareTag("Input"))
        {
            return vBlock.incomingCable != null && vBlock.incomingCable != this;
        }

        FunctionOneInput functionOneInput = port.GetComponentInParent<FunctionOneInput>();
        if (functionOneInput != null && port.CompareTag("Input"))
        {
            return functionOneInput.input != null && functionOneInput.input != this;
        }

        OneInputNumberFunction oneInputNumberFunction = port.GetComponentInParent<OneInputNumberFunction>();
        if (oneInputNumberFunction != null && port.CompareTag("Input"))
        {
            return oneInputNumberFunction.input != null && oneInputNumberFunction.input != this;
        }

        return false;
    }

    public bool ConnectToPort(Transform port)
    {
        if (port == null) return false;

        if (PortAlreadyOccupied(port))
        {
            return false;
        }

        DisconnectFromPort();
        connectedPort = port;

        TwoInputFunction bloqueFunc = port.GetComponentInParent<TwoInputFunction>();
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

            return true;
        }

        NumberBlock bloqueNum = port.GetComponentInParent<NumberBlock>();
        if (bloqueNum != null && port.CompareTag("Input"))
        {
            bloqueNum.incomingCable = this;
            return true;
        }

        VectorBlock vBlock = port.GetComponentInParent<VectorBlock>();
        if (vBlock != null && port.CompareTag("Input"))
        {
            vBlock.incomingCable = this;
            return true;
        }

        FunctionOneInput functionOneInput = port.GetComponentInParent<FunctionOneInput>();
        if (functionOneInput != null && port.CompareTag("Input"))
        {
            functionOneInput.input = this;
            return true;
        }

        OneInputNumberFunction oneInputNumberFunction = port.GetComponentInParent<OneInputNumberFunction>();
        if (oneInputNumberFunction != null && port.CompareTag("Input"))
        {
            oneInputNumberFunction.input = this;
            return true;
        }
        return false;
    }

    public void DisconnectFromPort()
    {
        if (connectedPort == null) return;

        TwoInputFunction bloqueFunc = connectedPort.GetComponentInParent<TwoInputFunction>();
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
        FunctionOneInput functionOneInput = connectedPort.GetComponentInParent<FunctionOneInput>();
        if (functionOneInput != null && connectedPort.CompareTag("Input") && functionOneInput.input == this)
        {
            functionOneInput.input = null;
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

        OneInputNumberFunction oneInputNumberFunction = connectedPort.GetComponentInParent<OneInputNumberFunction>();
        if (oneInputNumberFunction != null && connectedPort.CompareTag("Input") && oneInputNumberFunction.input == this)
        {
            oneInputNumberFunction.input = null;
        }

        connectedPort = null;
    }

    public bool IsNumberSource()
    {
        if (sourceObject == null) return false;

        if (sourceObject.GetComponent<NumberBlock>() != null) return true;

        TwoInputFunction twoInput = sourceObject.GetComponent<TwoInputFunction>();
        if (twoInput != null) return twoInput.OutputsNumber();

        FunctionOneInput oneInput = sourceObject.GetComponent<FunctionOneInput>();
        if (oneInput != null) return oneInput.OutputsNumber();

        OneInputNumberFunction oneInputNumber = sourceObject.GetComponent<OneInputNumberFunction>();
        if (oneInputNumber != null) return oneInputNumber.OutputsNumber();

        return false;
    }

    public bool IsVectorSource()
    {
        if (sourceObject == null) return false;

        if (sourceObject.GetComponent<VectorBlock>() != null) return true;

        TwoInputFunction twoInput = sourceObject.GetComponent<TwoInputFunction>();
        if (twoInput != null) return twoInput.OutputsVector();

        FunctionOneInput oneInput = sourceObject.GetComponent<FunctionOneInput>();
        if (oneInput != null) return oneInput.OutputsVector();

        return false;
    }

}
