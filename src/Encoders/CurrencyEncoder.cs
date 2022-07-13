using System.Text;

namespace dBASE.NET.Encoders;

internal class CurrencyEncoder : IEncoder
{
    private static CurrencyEncoder _instance;

    private CurrencyEncoder() { }

    public static CurrencyEncoder Instance => _instance ??= new CurrencyEncoder();

    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        float value = 0;
        if (data != null) value = (float)data;
        return BitConverter.GetBytes(value);
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        return BitConverter.ToSingle(buffer, 0);
    }
}