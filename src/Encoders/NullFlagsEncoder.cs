using System.Text;

namespace dBASE.NET.Encoders;

internal class NullFlagsEncoder : IEncoder
{
    private static NullFlagsEncoder _instance;

    private NullFlagsEncoder() { }

    public static NullFlagsEncoder Instance => _instance ??= new NullFlagsEncoder();

    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        var buffer = new byte[1];
        buffer[0] = 0;
        return buffer;
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        return buffer[0];
    }
}