using UnityEngine;

public enum OperationOutputType
{
    Number,
    Vector
}

public abstract class OneInputVectorOperation
{
    public abstract string Label { get; }
    public abstract OperationOutputType OutputType { get; }

    public string InputLabel => "V";
    public string OutputLabel => OutputType == OperationOutputType.Number ? "N" : "V";

    public virtual float ExecuteNumber(Vector3 input)
    {
        return 0f;
    }

    public virtual Vector3 ExecuteVector(Vector3 input)
    {
        return Vector3.zero;
    }
}

public class MagnitudeOperation : OneInputVectorOperation
{
    public override string Label => "MAGNITUD";
    public override OperationOutputType OutputType => OperationOutputType.Number;

    public override float ExecuteNumber(Vector3 input)
    {
        return MathOperations.Magnitude(input);
    }
}

public class NormalizeOperation : OneInputVectorOperation
{
    public override string Label => "NORMALIZACIÓN";
    public override OperationOutputType OutputType => OperationOutputType.Vector;

    public override Vector3 ExecuteVector(Vector3 input)
    {
        return MathOperations.Normalize(input);
    }
}

public class OppositeOperation : OneInputVectorOperation
{
    public override string Label => "OPUESTO";
    public override OperationOutputType OutputType => OperationOutputType.Vector;

    public override Vector3 ExecuteVector(Vector3 input)
    {
        return MathOperations.Opposite(input);
    }
}

public class AbsoluteOperation : OneInputVectorOperation
{
    public override string Label => "ABSOLUTO";
    public override OperationOutputType OutputType => OperationOutputType.Vector;

    public override Vector3 ExecuteVector(Vector3 input)
    {
        return MathOperations.Absolute(input);
    }
}