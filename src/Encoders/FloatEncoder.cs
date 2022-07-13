using System.Globalization;
using System.Text;

namespace dBASE.NET.Encoders;

internal class FloatEncoder : IEncoder
{
    private static FloatEncoder instance;

    private FloatEncoder() { }

    public static FloatEncoder Instance => instance ??= new FloatEncoder();

    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        var text = Convert.ToString(data, CultureInfo.InvariantCulture)?.PadLeft(field.Length, ' ');
        if (text != null && text.Length > field.Length)
        {
            text = text[..field.Length];
        }

        return text == null ? Array.Empty<byte>() : encoding.GetBytes(text);
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        var text = encoding.GetString(buffer).Trim();
        if (text.Length == 0)
        {
            return null;
        }

        return Convert.ToSingle(text, CultureInfo.InvariantCulture);
    }
}