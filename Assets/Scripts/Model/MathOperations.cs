using UnityEngine;

public static class MathOperations
{
    public static float Add(float a, float b)
    {
        return a + b;
    }

    public static float Subtract(float a, float b)
    {
        return a - b;
    }

    public static float Multiply(float a, float b)
    {
        return a * b;
    }

    public static float Divide(float a, float b)
    {
        return b != 0f ? a / b : 0f;
    }

    public static Vector3 Add(Vector3 a, Vector3 b)
    {
        return a + b;
    }

    public static Vector3 Subtract(Vector3 a, Vector3 b)
    {
        return a - b;
    }

    public static Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.x * b.x,
            a.y * b.y,
            a.z * b.z
        );
    }

    public static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(
            b.x != 0f ? a.x / b.x : 0f,
            b.y != 0f ? a.y / b.y : 0f,
            b.z != 0f ? a.z / b.z : 0f
        );
    }

    public static Vector3 Add(Vector3 vector, float scalar)
    {
        return vector + new Vector3(scalar, scalar, scalar);
    }

    public static Vector3 Add(float scalar, Vector3 vector)
    {
        return Add(vector, scalar);
    }

    public static Vector3 Subtract(Vector3 vector, float scalar)
    {
        return vector - new Vector3(scalar, scalar, scalar);
    }

    public static Vector3 Subtract(float scalar, Vector3 vector)
    {
        return new Vector3(scalar, scalar, scalar) - vector;
    }

    public static Vector3 Multiply(Vector3 vector, float scalar)
    {
        return vector * scalar;
    }

    public static Vector3 Multiply(float scalar, Vector3 vector)
    {
        return vector * scalar;
    }

    public static Vector3 Divide(Vector3 vector, float scalar)
    {
        return scalar != 0f ? vector / scalar : Vector3.zero;
    }

    public static Vector3 Divide(float scalar, Vector3 vector)
    {
        return new Vector3(
            vector.x != 0f ? scalar / vector.x : 0f,
            vector.y != 0f ? scalar / vector.y : 0f,
            vector.z != 0f ? scalar / vector.z : 0f
        );
    }

    public static float Magnitude(Vector3 vector)
    {
        return vector.magnitude;
    }

    public static Vector3 Normalize(Vector3 vector)
    {
        return vector != Vector3.zero ? vector.normalized : Vector3.zero;
    }

    public static Vector3 Opposite(Vector3 vector)
    {
        return -vector;
    }

    public static Vector3 Absolute(Vector3 vector)
    {
        return new Vector3(
            Mathf.Abs(vector.x),
            Mathf.Abs(vector.y),
            Mathf.Abs(vector.z)
        );
    }

    public static float Dot(Vector3 a, Vector3 b)
    {
        return Vector3.Dot(a, b);
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return Vector3.Cross(a, b);
    }

    public static Vector3 MidPoint(Vector3 a, Vector3 b)
    {
        return (a + b) / 2f;
    }

    public static float Angle(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b);
    }

    public static float SinDegrees(float angle)
    {
        return Mathf.Sin(angle * Mathf.Deg2Rad);
    }

    public static float CosDegrees(float angle)
    {
        return Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    public static float TanDegrees(float angle)
    {
        return Mathf.Tan(angle * Mathf.Deg2Rad);
    }

    public static float AsinDegrees(float value)
    {
        return Mathf.Asin(Mathf.Clamp(value, -1f, 1f)) * Mathf.Rad2Deg;
    }

    public static float AcosDegrees(float value)
    {
        return Mathf.Acos(Mathf.Clamp(value, -1f, 1f)) * Mathf.Rad2Deg;
    }

    public static float AtanDegrees(float value)
    {
        return Mathf.Atan(value) * Mathf.Rad2Deg;
    }
}