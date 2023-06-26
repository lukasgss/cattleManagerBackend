using System.Globalization;
using System.Text;

namespace CattleManager.Application.Application.Helpers;

public static class StringExtensions
{
    public static string RemoveDiacritics(string accentedStr)
    {
        var normalizedString = accentedStr.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }

    public static string AddDatabaseWildcards(string str)
    {
        string[] splittedString = str.Split(" ");
        string wildCardedString = string.Empty;
        foreach (string splitString in splittedString)
        {
            wildCardedString += $"%{string.Concat(splitString)}% ";
        }
        return wildCardedString;
    }
}