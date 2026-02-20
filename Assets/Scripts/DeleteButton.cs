using UnityEngine;
using Oculus.Interaction;

public class DeleteButton : MonoBehaviour
{
    [Header("References")]
    public CableMeta plugConnection; // La esfera del cable de este bloque
    public Grabbable grabbableButton;       // El Grabbable del propio botoncito

    private Vector3 fixedPosition;
    private Quaternion fixedRotation;

    void Start()
    {
        // Guardamos su posición exacta en la esquina al empezar
        fixedPosition = transform.localPosition;
        fixedRotation = transform.localRotation;

        if (grabbableButton != null)
        {
            // Suscribimos al mismo sistema de eventos que usas en el cable
            grabbableButton.WhenPointerEventRaised += EventsButton;
        }
    }

    private void OnDestroy()
    {
        if (grabbableButton != null)
            grabbableButton.WhenPointerEventRaised -= EventsButton;
    }

    private void EventsButton(PointerEvent evento)
    {
        // Detectamos el SELECT (cuando cierras el puño o pulsas el gatillo sobre él)
        if (evento.Type == PointerEventType.Select)
        {
            DeleteSecurity();
        }
    }

    // Forzamos que el botón se quede en su esquina cada frame 
    // por si el Grabbable intenta moverlo un poco
    void Update()
    {
        transform.localPosition = fixedPosition;
        transform.localRotation = fixedRotation;
    }

    void DeleteSecurity()
    {
        if (plugConnection == null) return;

        Transform rootBlock = plugConnection.blockOriginal;

        // 1. Limpiamos cables ajenos conectados a este bloque
        CableMeta[] allCables = FindObjectsOfType<CableMeta>();
        foreach (CableMeta c in allCables)
        {
            // Si el cable está conectado a algún hijo de mi bloque...
            if (c.transform.IsChildOf(rootBlock))
            {
                c.ResetPosition();
            }
        }

        // 2. Reseteamos nuestro propio cable para que vuelva al padre
        plugConnection.ResetPosition();

        // 3. Destruimos el bloque completo
        Destroy(rootBlock.gameObject);
    }
}