public abstract class OneInputNumberOperation
{
    public abstract string Label { get; }

    public virtual string InputLabel => "N";
    public virtual string OutputLabel => "N";

    public abstract float Execute(float input);
}

public class SinOperation : OneInputNumberOperation
{
    public override string Label => "SENO";

    public override float Execute(float input)
    {
        return MathOperations.SinDegrees(input);
    }
}

public class CosOperation : OneInputNumberOperation
{
    public override string Label => "COSENO";

    public override float Execute(float input)
    {
        return MathOperations.CosDegrees(input);
    }
}

public class TanOperation : OneInputNumberOperation
{
    public override string Label => "TANGENTE";

    public override float Execute(float input)
    {
        return MathOperations.TanDegrees(input);
    }
}

public class AsinOperation : OneInputNumberOperation
{
    public override string Label => "ARCOSENO";

    public override float Execute(float input)
    {
        return MathOperations.AsinDegrees(input);
    }
}

public class AcosOperation : OneInputNumberOperation
{
    public override string Label => "ARCOCOSENO";

    public override float Execute(float input)
    {
        return MathOperations.AcosDegrees(input);
    }
}

public class AtanOperation : OneInputNumberOperation
{
    public override string Label => "ARCOTANGENTE";

    public override float Execute(float input)
    {
        return MathOperations.AtanDegrees(input);
    }
}