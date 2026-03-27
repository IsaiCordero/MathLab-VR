using UnityEngine;
using Oculus.Interaction;
public class SpawnerBloques : MonoBehaviour
{
    public GameObject prefabBloque;
    public Grabbable grabbableBoton;
    private Vector3 fixedPost;
    private Quaternion fixedLocalRot;

    void Start()
    {
        fixedLocalRot = transform.localRotation;
        fixedPost = transform.localPosition;
        grabbableBoton.WhenPointerEventRaised += (evento) =>
        {
          if( evento.Type == PointerEventType.Select)
            {
                Spawn();
            }  
        };
    }

    void LateUpdate()
    {
        transform.localPosition = fixedPost;
        transform.localRotation = fixedLocalRot;
    } 

    void Spawn()
    {
        GameObject newBloque = Instantiate(prefabBloque, transform.position, Quaternion.identity);
        newBloque.transform.position += transform.forward * 0.2f;
    }
    
}
