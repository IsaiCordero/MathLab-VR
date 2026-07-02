using UnityEngine;

public abstract class TwoInputOperation
{
    public abstract string Label { get; }
    public abstract string FirstInputLabel { get; }
    public abstract string SecondInputLabel { get; }
    public virtual bool UsesNeutralColor => false;
    public abstract bool AcceptsInput(NodeValueType inputType);
    public abstract NodeValueType GetOutputType(NodeValueType firstType, NodeValueType secondType);
    public abstract NodeValue Execute(NodeValue first, NodeValue second);
}

public abstract class BasicTwoInputOperation : TwoInputOperation
{
    public override string FirstInputLabel => "N/V";
    public override string SecondInputLabel => "N/V";
    public override bool UsesNeutralColor => true;
    public override bool AcceptsInput(NodeValueType inputType)
    {
        return inputType == NodeValueType.Number || inputType == NodeValueType.Vector;
    }

    public override NodeValueType GetOutputType(NodeValueType firstType, NodeValueType secondType)
    {
        return firstType == NodeValueType.Vector || secondType == NodeValueType.Vector
            ? NodeValueType.Vector
            : NodeValueType.Number;
    }
}

public class AddTwoInputOperation : BasicTwoInputOperation
{
    public override string Label => "SUMA";

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (first.IsNumber && second.IsNumber)
            return NodeValue.FromNumber(MathOperations.Add(first.Number, second.Number));

        if (first.IsVector && second.IsVector)
            return NodeValue.FromVector(MathOperations.Add(first.Vector, second.Vector));

        if (first.IsVector && second.IsNumber)
            return NodeValue.FromVector(MathOperations.Add(first.Vector, second.Number));

        return NodeValue.FromVector(MathOperations.Add(first.Number, second.Vector));
    }
}

public class SubtractTwoInputOperation : BasicTwoInputOperation
{
    public override string Label => "RESTA";

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (first.IsNumber && second.IsNumber)
            return NodeValue.FromNumber(MathOperations.Subtract(first.Number, second.Number));

        if (first.IsVector && second.IsVector)
            return NodeValue.FromVector(MathOperations.Subtract(first.Vector, second.Vector));

        if (first.IsVector && second.IsNumber)
            return NodeValue.FromVector(MathOperations.Subtract(first.Vector, second.Number));

        return NodeValue.FromVector(MathOperations.Subtract(first.Number, second.Vector));
    }
}

public class MultiplyTwoInputOperation : BasicTwoInputOperation
{
    public override string Label => "MULTIPLICACION";

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (first.IsNumber && second.IsNumber)
            return NodeValue.FromNumber(MathOperations.Multiply(first.Number, second.Number));

        if (first.IsVector && second.IsVector)
            return NodeValue.FromVector(MathOperations.Multiply(first.Vector, second.Vector));

        if (first.IsVector && second.IsNumber)
            return NodeValue.FromVector(MathOperations.Multiply(first.Vector, second.Number));

        return NodeValue.FromVector(MathOperations.Multiply(first.Number, second.Vector));
    }
}

public class DivideTwoInputOperation : BasicTwoInputOperation
{
    public override string Label => "DIVISION";

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (first.IsNumber && second.IsNumber)
            return NodeValue.FromNumber(MathOperations.Divide(first.Number, second.Number));

        if (first.IsVector && second.IsVector)
            return NodeValue.FromVector(MathOperations.Divide(first.Vector, second.Vector));

        if (first.IsVector && second.IsNumber)
            return NodeValue.FromVector(MathOperations.Divide(first.Vector, second.Number));

        return NodeValue.FromVector(MathOperations.Divide(first.Number, second.Vector));
    }
}

public class DotProductOperation : TwoInputOperation
{
    public override string Label => "PRODUCTO\nESCALAR";
    public override string FirstInputLabel => "V";
    public override string SecondInputLabel => "V";

    public override bool AcceptsInput(NodeValueType inputType)
    {
        return inputType == NodeValueType.Vector;
    }

    public override NodeValueType GetOutputType(NodeValueType firstType, NodeValueType secondType)
    {
        return NodeValueType.Number;
    }

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (!first.IsVector || !second.IsVector) return NodeValue.FromNumber(0f);
        return NodeValue.FromNumber(MathOperations.Dot(first.Vector, second.Vector));
    }
}

public class CrossProductOperation : TwoInputOperation
{
    public override string Label => "PRODUCTO\nVECTORIAL";
    public override string FirstInputLabel => "V";
    public override string SecondInputLabel => "V";

    public override bool AcceptsInput(NodeValueType inputType)
    {
        return inputType == NodeValueType.Vector;
    }

    public override NodeValueType GetOutputType(NodeValueType firstType, NodeValueType secondType)
    {
        return NodeValueType.Vector;
    }

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (!first.IsVector || !second.IsVector) return NodeValue.FromVector(Vector3.zero);
        return NodeValue.FromVector(MathOperations.Cross(first.Vector, second.Vector));
    }
}

public class MidPointOperation : TwoInputOperation
{
    public override string Label => "PUNTO\nMEDIO";
    public override string FirstInputLabel => "V";
    public override string SecondInputLabel => "V";

    public override bool AcceptsInput(NodeValueType inputType)
    {
        return inputType == NodeValueType.Vector;
    }

    public override NodeValueType GetOutputType(NodeValueType firstType, NodeValueType secondType)
    {
        return NodeValueType.Vector;
    }

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (!first.IsVector || !second.IsVector) return NodeValue.FromVector(Vector3.zero);
        return NodeValue.FromVector(MathOperations.MidPoint(first.Vector, second.Vector));
    }
}

public class AngleOperation : TwoInputOperation
{
    public override string Label => "ANGULO";
    public override string FirstInputLabel => "V";
    public override string SecondInputLabel => "V";

    public override bool AcceptsInput(NodeValueType inputType)
    {
        return inputType == NodeValueType.Vector;
    }

    public override NodeValueType GetOutputType(NodeValueType firstType, NodeValueType secondType)
    {
        return NodeValueType.Number;
    }

    public override NodeValue Execute(NodeValue first, NodeValue second)
    {
        if (!first.IsVector || !second.IsVector) return NodeValue.FromNumber(0f);
        return NodeValue.FromNumber(MathOperations.Angle(first.Vector, second.Vector));
    }
}