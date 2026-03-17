using UnityEngine;

public class DataCable : MonoBehaviour
{
    public GameObject sourceObject;

    public float GetValueFromSource()
    {
        if(sourceObject == null)
        {
            return 0;
        }

        NumberBlock numberBlock = sourceObject.GetComponent<NumberBlock>();
        if(numberBlock != null)
        {
            Debug.Log("Valor = " + numberBlock.currentValue);
            return numberBlock.currentValue;
        }

        SelectFunction selectFunction = sourceObject.GetComponent<SelectFunction>();
        if(selectFunction != null)
        {
            Debug.Log("Valor2 = " + selectFunction.GetCurrentResult());
            return selectFunction.GetCurrentResult();
        }

        return 0;
    }
}
