using System;
using System.Text.RegularExpressions;

namespace ShopCatalog14;

public static class RegexValidator
{
    private static readonly Regex NameRegex = new Regex(
        @"^[\p{L}\p{M}0-9][\p{L}\p{M}0-9\s.'’&()\-№#]{0,79}$",
        RegexOptions.Compiled);

    private static readonly Regex AddressRegex = new Regex(
        @"^[\p{L}\p{M}0-9][\p{L}\p{M}0-9\s.,'’/\\\-№#]{0,149}$",
        RegexOptions.Compiled);

    private static readonly Regex PhoneRegex = new Regex(
        @"^(?:\+?380|0)(?:39|50|63|66|67|68|73|75|77|91|92|93|94|95|96|97|98|99)\d{7}$",
        RegexOptions.Compiled);

    private static readonly Regex TimeRegex = new Regex(
        @"^([01]\d|2[0-3]):[0-5]\d-([01]\d|2[0-3]):[0-5]\d$",
        RegexOptions.Compiled);

    private static readonly Regex FoodRegex = new Regex(
        @"продукт|продоволь|супермаркет|гастроном|бакале|овоч|фрукт|м'яс|мяс|молоч|хліб|хлеб|food|grocery|supermarket|groceries",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidName(string value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               NameRegex.IsMatch(value.Trim());
    }

    public static bool IsValidAddress(string value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
               AddressRegex.IsMatch(value.Trim());
    }

    public static string NormalizePhone(string value)
    {
        if (value == null)
            return "";

        return value.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");
    }

    public static bool IsValidPhone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return PhoneRegex.IsMatch(NormalizePhone(value));
    }

    public static bool IsValidWorkingHours(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        string normalized = value.Trim();

        if (!TimeRegex.IsMatch(normalized))
            return false;

        string[] parts = normalized.Split('-');

        TimeSpan start = TimeSpan.Parse(parts[0]);
        TimeSpan end = TimeSpan.Parse(parts[1]);

        return end > start;
    }

    public static bool IsClosedDay(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        string normalized = value.Trim();

        return normalized.Equals(
                   "Closed",
                   StringComparison.OrdinalIgnoreCase) ||
               normalized.Equals(
                   "Вихідний",
                   StringComparison.OrdinalIgnoreCase) ||
               normalized.Equals(
                   "Выходной",
                   StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsValidScheduleEntry(string value)
    {
        if (IsClosedDay(value))
            return true;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        string[] intervals = value.Trim().Split(',');

        if (intervals.Length < 1 || intervals.Length > 2)
            return false;

        foreach (string interval in intervals)
        {
            if (!IsValidWorkingHours(interval.Trim()))
                return false;
        }

        if (intervals.Length == 2)
        {
            string firstEnd = intervals[0].Trim().Split('-')[1];
            string secondStart = intervals[1].Trim().Split('-')[0];

            TimeSpan end = TimeSpan.Parse(firstEnd);
            TimeSpan start = TimeSpan.Parse(secondStart);

            if (start < end)
                return false;
        }

        return true;
    }

    public static bool IsFoodSpecialization(string specialization)
    {
        if (string.IsNullOrWhiteSpace(specialization))
            return false;

        return FoodRegex.IsMatch(specialization);
    }
}