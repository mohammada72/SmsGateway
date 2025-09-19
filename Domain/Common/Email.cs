using System.Text.RegularExpressions;

namespace Domain.Common
{
    public readonly struct Email : IEquatable<Email>
    {
        private readonly string _value;

        public Email(string value)
        {
            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format", nameof(value));

            _value = value;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static implicit operator Email(string value) => new(value);

        public static implicit operator string(Email email) => email._value;

        public override string ToString() => _value;

        public bool Equals(Email other) =>
            string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) =>
            obj is Email other && Equals(other);

        public override int GetHashCode() =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(_value);

        public static bool operator ==(Email left, Email right) => left.Equals(right);
        public static bool operator !=(Email left, Email right) => !left.Equals(right);

    }
}
