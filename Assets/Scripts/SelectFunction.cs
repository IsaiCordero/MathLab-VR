using UnityEngine;
using TMPro;

public class SelectFunction : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI visualText; // Arrastra aquí el texto del Canvas
    
    [Header("Settings")]
    public string[] functions = { "SUMA", "RESTA", "MULTIPLICACIÓN", "DIVISIÓN" };
    public float cooldown = 0.5f; // Tiempo de espera para evitar toques dobles
    
    private int actual = 0;
    private float lastTouchTime;

    void Start()
    {
        // Al empezar, nos aseguramos de que el texto coincida con el primer elemento
        if (visualText != null && functions.Length > 0)
        {
            visualText.text = functions[actual];
        }
    }

    public void ChangeNextFunction()
    {
        actual++;
        
        // Si llegamos al final de la lista, volvemos al principio (0)
        if (actual >= functions.Length) 
        {
            actual = 0;
        }

        visualText.text = functions[actual];
        Debug.Log("Función cambiada a: " + functions[actual]);
    }

    // ESTO ES LO QUE HEMOS AÑADIDO:
    // Se activa cuando el dedo (con Collider) entra en el texto (con Collider + Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        // 1. Verificamos que ha pasado el tiempo de seguridad
        if (Time.time > lastTouchTime + cooldown)
        {
            // 2. Filtramos para que solo cambie si lo toca algo relacionado con la mano
            if (other.name.Contains("Hand") || other.name.Contains("Finger") || other.name.Contains("Index"))
            {
                ChangeNextFunction();
                lastTouchTime = Time.time; // Guardamos el momento del toque
            }
        }
    }
}