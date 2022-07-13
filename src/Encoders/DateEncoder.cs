using System.Globalization;
using System.Text;

namespace dBASE.NET.Encoders;

internal class DateEncoder : IEncoder
{
    private const string Format = "yyyyMMdd";

    private static DateEncoder _instance;

    private DateEncoder() { }

    public static DateEncoder Instance => _instance ??= new DateEncoder();

    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        string text;
        if (data is DateTime dt)
        {
            text = dt.ToString(Format).PadLeft(field.Length, ' ');
        }
        else
        {
            text = field.DefaultValue;
        }

        return encoding.GetBytes(text);
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        var text = encoding.GetString(buffer).Trim();
        if (text.Length == 0)
        {
            return null;
        }
        return DateTime.ParseExact(text, Format, CultureInfo.InvariantCulture);
    }
}