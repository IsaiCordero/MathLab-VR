using UnityEngine;

public enum NodeValueType
{
    Number,
    Vector
}

public struct NodeValue
{
    public NodeValueType Type;
    public float Number;
    public Vector3 Vector;

    public bool IsNumber => Type == NodeValueType.Number;
    public bool IsVector => Type == NodeValueType.Vector;

    public static NodeValue FromNumber(float value)
    {
        return new NodeValue
        {
            Type = NodeValueType.Number,
            Number = value,
            Vector = Vector3.zero
        };
    }

    public static NodeValue FromVector(Vector3 value)
    {
        return new NodeValue
        {
            Type = NodeValueType.Vector,
            Number = 0f,
            Vector = value
        };
    }
}