using UnityEngine;
using Oculus.Interaction;

public class ButtonSelector : MonoBehaviour
{
    [Header("References")]
    public SelectFunction scriptLogic;

    private Grabbable grabbable;
    
    // Guardamos la posición y rotación fijas
    private Vector3 fixedLocalPos;
    private Quaternion fixedLocalRot;

    void Start()
    {
        // Al inicio, memorizamos dónde debe quedarse la esfera
        fixedLocalPos = transform.localPosition;
        fixedLocalRot = transform.localRotation;
        
        grabbable = GetComponent<Grabbable>();

        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandlePointerEvent;
        }
    }

    // Usamos LateUpdate porque se ejecuta DESPUÉS de que el SDK de Oculus
    // haya intentado mover la esfera. Así la "teletransportamos" de vuelta.
    void LateUpdate()
    {
        transform.localPosition = fixedLocalPos;
        transform.localRotation = fixedLocalRot;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
        {
            if (scriptLogic != null)
            {
                scriptLogic.ChangeNextFunction();
                Debug.Log("¡Función cambiada! La esfera permanece inmóvil.");
            }
        }
    }

    private void OnDestroy()
    {
        if (grabbable != null)
            grabbable.WhenPointerEventRaised -= HandlePointerEvent;
    }
}