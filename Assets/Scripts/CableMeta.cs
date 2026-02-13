using UnityEngine;
using Oculus.Interaction;

public class CableMeta : MonoBehaviour
{
    [Header("Componentes")]
    public LineRenderer lineRenderer;
    public Grabbable grabbableMeta; 
    
    [Header("Configuración")]
    public Transform salidaFijaDelBloque; 
    public float distanciaConexion = 0.1f;
    public string tagEntrada = "PuertoEntrada"; 
[
    Header("Referencias de Jerarquía")]
    public Transform bloqueOrigenPadre; 
    public Transform miBloquePadre;
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;
    private Transform puertoDestino = null;

    void Start()
    {
        posicionOriginal = transform.localPosition;
        rotacionOriginal = transform.localRotation;

        grabbableMeta.WhenPointerEventRaised += GestionarEventosMeta;
    }

    private void OnDestroy()
    {
        if (grabbableMeta != null)
            grabbableMeta.WhenPointerEventRaised -= GestionarEventosMeta;
    }

    void Update()
    {
        if (salidaFijaDelBloque != null)
        {
            lineRenderer.positionCount = 2;

            lineRenderer.SetPosition(0, salidaFijaDelBloque.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void GestionarEventosMeta(PointerEvent evento)
    {
        if (evento.Type == PointerEventType.Unselect)
        {
            IntentarConectar();
        }
        else if (evento.Type == PointerEventType.Select)
        {
            puertoDestino = null;
        }
    }

    void IntentarConectar()
{
    Collider[] hits = Physics.OverlapSphere(transform.position, distanciaConexion);
    Transform entradaEncontrada = null;

    foreach (var hit in hits)
    {
        if (hit.CompareTag(tagEntrada))
        {
            if(hit.transform.IsChildOf(bloqueOrigenPadre))
                {
                    continue;
                }
            entradaEncontrada = hit.transform;
            break;
        }
    }

    if (entradaEncontrada != null)
    {
        transform.position = entradaEncontrada.position;
        transform.rotation = entradaEncontrada.rotation;

        transform.SetParent(entradaEncontrada); 
        
        puertoDestino = entradaEncontrada;
    }
    else
    {
        ResetearPosicion();
    }
}

void ResetearPosicion()
{
    transform.SetParent(bloqueOrigenPadre);
    
    transform.localPosition = posicionOriginal;
    transform.localRotation = rotacionOriginal;
    puertoDestino = null;
}
}