using UnityEngine;

public class VectorController : MonoBehaviour
{
    [Header("Limits")]
    public Vector3 minLocalPosition = new Vector3(-0.3f, -0.3f, -0.3f);
    public Vector3 maxLocalPosition = new Vector3(0.3f, 0.3f, 0.3f);

    private Vector3 lastLocalPosition;


    void Start()
    {
        lastLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (transform.localPosition == lastLocalPosition)
            return;

        Vector3 localPos = transform.localPosition;

        localPos.x = Mathf.Clamp(localPos.x, minLocalPosition.x, maxLocalPosition.x);
        localPos.y = Mathf.Clamp(localPos.y, minLocalPosition.y, maxLocalPosition.y);
        localPos.z = Mathf.Clamp(localPos.z, minLocalPosition.z, maxLocalPosition.z);

        transform.localPosition = localPos;
        lastLocalPosition = localPos;
    }

}
