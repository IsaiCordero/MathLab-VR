using UnityEngine;

public static class VectorPositionMapper
{
    public static Vector3 VectorToLocalPosition(Vector3 vector, Vector3 centerLocalPosition, float scaleFactor)
    {
        Vector3 localOffset = vector / scaleFactor;
        localOffset.x = -localOffset.x;

        return centerLocalPosition + localOffset;
    }

    public static Vector3 LocalPositionToVector(Vector3 localPosition, Vector3 centerLocalPosition, float scaleFactor)
    {
        Vector3 localOffset = localPosition - centerLocalPosition;
        localOffset.x = -localOffset.x;

        return localOffset * scaleFactor;
    }
}