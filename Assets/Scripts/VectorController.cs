using UnityEngine;

public class VectorController : MonoBehaviour
{
    public VectorBlock parentVectorBlock;
    public Transform areaCenter;

    [Header("Limits")]
    public Vector3 minLocalPosition = new Vector3(-0.3f, -0.3f, -0.3f);
    public Vector3 maxLocalPosition = new Vector3(0.3f, 0.3f, 0.3f);

    void Update()
    {
        Vector3 localPos = transform.localPosition;

        localPos.x = Mathf.Clamp(localPos.x, minLocalPosition.x, maxLocalPosition.x);
        localPos.y = Mathf.Clamp(localPos.y, minLocalPosition.y, maxLocalPosition.y);
        localPos.z = Mathf.Clamp(localPos.z, minLocalPosition.z, maxLocalPosition.z);

        transform.localPosition = localPos;

        if (parentVectorBlock != null)
        {
            if (areaCenter != null)
            {
                parentVectorBlock.currentVector = transform.localPosition - areaCenter.localPosition;
            }
            else
            {
                parentVectorBlock.currentVector = transform.localPosition;
            }
        }
    }
}
