using UnityEngine;
using Oculus.Interaction;
public class SpawnerBloques : MonoBehaviour
{
    public GameObject prefabBloque;
    public Grabbable grabbableBoton;

    void Start()
    {
        grabbableBoton.WhenPointerEventRaised += (evento) =>
        {
          if( evento.Type == PointerEventType.Select)
            {
                Spawn();
            }  
        };
    }

    void Spawn()
    {
        GameObject newBloque = Instantiate(prefabBloque, transform.position, Quaternion.identity);
        newBloque.transform.position += transform.forward * 0.2f;
    }
    
}
