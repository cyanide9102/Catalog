namespace Catalog.Core.Constants
{
    public enum StringCondition
    {
        Blank = 0,
        Equal,
        NotEqual,
        Contains,
        StartsWith,
        EndsWith
    }

    public enum NumberCondition
    {
        Blank = 0,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    public enum ValueCondition
    {
        Blank = 0,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }
}
