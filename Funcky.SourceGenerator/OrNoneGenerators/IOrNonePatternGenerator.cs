namespace Funcky.SourceGenerator.OrNoneGenerators
{
    internal interface IOrNonePatternGenerator
    {
        string HintName { get; }

        string Source { get; }
    }
}
