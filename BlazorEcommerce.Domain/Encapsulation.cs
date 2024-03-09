namespace BlazorEcommerce.Domain
{
    internal static class Encapsulation
    {
        internal static void ThrowIfNull<T>(T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), $"{nameof(value)} can not be set to null.");
            }
        }

        internal static void ThrowIfNullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(value)} can not be set to null.", nameof(value));
            }
        }

        internal static void ThrowIfStringIsTooLong(string value, int maxLength)
        {
            ThrowIfNullOrWhiteSpace(value);
            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{nameof(value)} can not be more than {maxLength} characters long.", nameof(value));
            }
        }

        internal static void ThrowIfStringIsTooShort(string value, int minLength)
        {
            ThrowIfNullOrWhiteSpace(value);
            if (value.Length < minLength)
            {
                throw new ArgumentException($"{nameof(value)} can not be less than {minLength} characters long.", nameof(value));
            }
        }

        internal static void ThrowIfZeroOrLess(int value)
        {
            ThrowIfNull(value);
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} can not be less than 0.");
            }
        }
    }
}
