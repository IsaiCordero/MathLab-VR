using UnityEngine;
using Oculus.Interaction; 

public class VectorController : MonoBehaviour
{
    public VectorBlock parentVectorBlock; 

    private Vector3 startPosition;
    private Transform originalParent;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false; 

        startPosition = transform.localPosition;
        originalParent = transform.parent;
    }

    void LateUpdate()
    {
        if (parentVectorBlock != null)
        {
            parentVectorBlock.currentVector = transform.localPosition;
        }
    }
}