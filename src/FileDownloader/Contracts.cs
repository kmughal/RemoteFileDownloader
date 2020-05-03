namespace FileDownloader
{
    using System;

    public static class Contracts
    {
        public static void NullCheck<T>(T value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));

            if (value is string && string.IsNullOrWhiteSpace(value as string)) throw new ArgumentNullException(nameof(value));
        }
    }
}
