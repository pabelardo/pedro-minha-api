namespace Business.Utils;

public static class Helpers
{
    public static string ApenasNumeros(string valor)
    {
        var onlyNumber = valor.Where(char.IsDigit).Aggregate("", (current, s) => current + s);

        return onlyNumber.Trim();
    }
}