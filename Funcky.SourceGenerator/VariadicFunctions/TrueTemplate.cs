namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal sealed class TrueTemplate : ConstantFunctionTemplate
    {
        public override string FormatMethodName() => "True";

        public override string FormatReturnValue() => "true";
    }
}
