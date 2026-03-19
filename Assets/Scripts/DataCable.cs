using UnityEngine;

public class DataCable : MonoBehaviour
{
    public GameObject sourceObject;

    // 1. LÓGICA DE LECTURA (Lo que ya tenías)
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

    // 2. LÓGICA DE CONEXIÓN AUTOMÁTICA (Lo que falta para el Header)
    // Este método se ejecuta cuando la punta del cable toca un "agujero"
    private void OnTriggerEnter(Collider other)
    {
        // Buscamos si el objeto que tocamos pertenece a un bloque de funciones
        SelectFunction bloqueFunc = other.GetComponentInParent<SelectFunction>();

        if (bloqueFunc != null)
        {
            if (other.CompareTag("First InPut"))
            {
                bloqueFunc.firstInput = this; // Se asigna al Header automáticamente

            }
            else if (other.CompareTag("Second InPut"))
            {
                bloqueFunc.secondInput = this; // Se asigna al Header automáticamente

            }
        }

        // También para el bloque de número normal (el que ya te funcionaba)
        NumberBlock bloqueNum = other.GetComponentInParent<NumberBlock>();
        if (bloqueNum != null && other.CompareTag("Input"))
        {
            bloqueNum.incomingCable = this;

        }
    }

}