using System.Globalization;
using System.Text;

namespace dBASE.NET.Encoders;

internal class NumericEncoder : IEncoder
{
    private static NumericEncoder _instance;

    private NumericEncoder() { }

    public static NumericEncoder Instance => _instance ??= new NumericEncoder();

    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        var text = Convert.ToString(data, CultureInfo.InvariantCulture);
        if (string.IsNullOrEmpty(text))
        {
            text = field.DefaultValue;
        }
        else
        {
            var parts = text.Split('.');
            if (parts.Length == 2)
            {
                // Truncate or pad float part.
                if (parts[1].Length > field.Precision)
                {
                    parts[1] = parts[1].Substring(0, field.Precision);
                }
                else
                {
                    parts[1] = parts[1].PadRight(field.Precision, '0');
                }
            }
            else if (field.Precision > 0)
            {
                // If value has no fractional part, pad it with zeros.
                parts = new[] { parts[0], new string('0', field.Precision) };
            }

            text = string.Join(".", parts);

            // Pad string with spaces or trim.
            text = text.Length > field.Length
                ? text.Substring(0, field.Length)
                : text.PadLeft(field.Length, ' ');
        }

        return encoding.GetBytes(text);
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        string text = encoding.GetString(buffer).Trim();
        if (text.Length == 0)
        {
            return null;
        }

        return Convert.ToDouble(text, CultureInfo.InvariantCulture);
    }
}