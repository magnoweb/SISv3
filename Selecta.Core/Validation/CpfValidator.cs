using System.Text.RegularExpressions;

namespace Selecta.Core.Validation;

/// <summary>Porta fiel de Selecta.Infra.Shared.Helpers.ValidaCpf (dígitos verificadores).</summary>
public static partial class CpfValidator
{
    public static bool IsValid(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        cpf = DigitsOnly(cpf);
        if (cpf.Length > 11) return false;

        cpf = cpf.PadLeft(11, '0');

        if (cpf.Distinct().Count() == 1 || cpf == "12345678909")
            return false;

        var digits = cpf.Select(c => c - '0').ToArray();

        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += (10 - i) * digits[i];

        var remainder = sum % 11;
        var firstCheck = remainder is 0 or 1 ? 0 : 11 - remainder;
        if (digits[9] != firstCheck) return false;

        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += (11 - i) * digits[i];

        remainder = sum % 11;
        var secondCheck = remainder is 0 or 1 ? 0 : 11 - remainder;
        return digits[10] == secondCheck;
    }

    private static string DigitsOnly(string text) => NonDigitRegex().Replace(text, string.Empty);

    [GeneratedRegex("[^0-9]")]
    private static partial Regex NonDigitRegex();
}
