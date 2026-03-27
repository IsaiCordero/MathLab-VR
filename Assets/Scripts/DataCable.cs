using UnityEngine;

public class DataCable : MonoBehaviour
{
    public GameObject sourceObject;

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

    private void OnTriggerEnter(Collider other)
    {
        SelectFunction bloqueFunc = other.GetComponentInParent<SelectFunction>();

        if (bloqueFunc != null)
        {
            if (other.CompareTag("First InPut"))
            {
                bloqueFunc.firstInput = this; 

            }
            else if (other.CompareTag("Second InPut"))
            {
                bloqueFunc.secondInput = this; 

            }
        }
        NumberBlock bloqueNum = other.GetComponentInParent<NumberBlock>();
        if (bloqueNum != null && other.CompareTag("Input"))
        {
            bloqueNum.incomingCable = this;

        }
    }

}