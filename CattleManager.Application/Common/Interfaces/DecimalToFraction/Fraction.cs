namespace CattleManager.Application.Application.Common.Interfaces.DecimalToFraction;

public readonly struct Fraction
{
    public int Numerator { get; }
    public int Denominator { get; }

    public Fraction(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }
}