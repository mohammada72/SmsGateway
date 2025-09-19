namespace Domain.Common;

public readonly struct NationalId : IEquatable<NationalId>
{
    private readonly string _value;

    public NationalId(string value)
    {
        if (!IsValidNationalId(value))
            throw new ArgumentException("Invalid National ID", nameof(value));

        _value = value;
    }

    public static bool IsValidNationalId(string nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
            return false;

        nationalId = new string(nationalId.Where(char.IsDigit).ToArray());

        if (nationalId.Length != 10)
            return false;

        if (nationalId.Distinct().Count() == 1)
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = nationalId[i] - '0';
            sum += digit * (10 - i);
        }

        int remainder = sum % 11;
        int controlDigit = nationalId[9] - '0';

        int expectedControlDigit = remainder < 2 ? remainder : 11 - remainder;

        return controlDigit == expectedControlDigit;
    }

    public static implicit operator NationalId(string value) => new NationalId(value);

    public static implicit operator string(NationalId nationalId) => nationalId._value;

    public override string ToString() => _value;

    public string ToFormattedString() => _value.Insert(3, "-").Insert(7, "-");

    public bool Equals(NationalId other) =>
        string.Equals(_value, other._value, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is NationalId other && Equals(other);

    public override int GetHashCode() =>
        _value?.GetHashCode() ?? 0;

    public static bool operator ==(NationalId left, NationalId right) => left.Equals(right);
    public static bool operator !=(NationalId left, NationalId right) => !left.Equals(right);

}
