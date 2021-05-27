namespace Funcky.SourceGenerator.VariadicFunctions
{
    internal sealed class FalseTemplate : ConstantFunctionTemplate
    {
        public override string FormatMethodName() => "False";

        public override string FormatReturnValue() => "false";
    }
}
