using System;
using CattleManager.Application.Application.Services.General;
using Xunit;

namespace CattleManager.Tests;

public class DecimalToFractionServiceTests
{
    [Fact]
    public void Returns_1_If_Numerator_And_Denominator_Are_Equal()
    {
        const double value = 5 / 5;

        var response = DecimalToFractionService.RealToFraction(value);

        Assert.Equal(1, response.Numerator);
        Assert.Equal(1, response.Denominator);
    }

    [Fact]
    public void Returns_0_If_Value_Is_0()
    {
        const int value = 0;

        var response = DecimalToFractionService.RealToFraction(value);

        Assert.Equal(0, response.Numerator);
        Assert.Equal(1, response.Denominator);
    }

    [Fact]
    public void Returns_Correct_Value_Of_Most_Common_Breed_Fractions()
    {
        double[] mostCommonBreedFractions = new[] { 1.0, 1.0 / 2, 1.0 / 4, 1.0 / 8, 3.0 / 4, 3.0 / 8, 5.0 / 8, 7.0 / 8 };

        double[] generatedFractions = new double[mostCommonBreedFractions.Length];
        for (int i = 0; i < generatedFractions.Length; i++)
        {
            var result = DecimalToFractionService.RealToFraction(mostCommonBreedFractions[i]);
            generatedFractions[i] = (double)result.Numerator / result.Denominator;
        }

        for (int i = 0; i < generatedFractions.Length; i++)
            Assert.Equal(mostCommonBreedFractions[i], generatedFractions[i]);
    }

    [Fact]
    public void Returns_Correct_Fraction_For_Periodic_Decimals()
    {
        double[] periodicDecimals = new[] { 1.0 / 3, 1.0 / 6, 1.0 / 12, 2.0 / 3, 5.0 / 6, 5.0 / 9, 5.0 / 9, 7.0 / 9 };

        double[] generatedFractions = new double[periodicDecimals.Length];
        for (int i = 0; i < generatedFractions.Length; i++)
        {
            var result = DecimalToFractionService.RealToFraction(periodicDecimals[i]);
            generatedFractions[i] = (double)result.Numerator / result.Denominator;
        }

        for (int i = 0; i < generatedFractions.Length; i++)
            Assert.Equal(periodicDecimals[i], generatedFractions[i]);
    }
}