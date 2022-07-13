﻿using System.Text;

namespace dBASE.NET.Encoders;

#pragma warning disable 1591
public class IntegerEncoder : IEncoder
{
    private static IntegerEncoder _instance;

    private IntegerEncoder() { }

    public static IntegerEncoder Instance => _instance ??= new IntegerEncoder();
#pragma warning restore 1591
    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        var value = 0;
        if (data != null) value = (int)data;
        return BitConverter.GetBytes(value);
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        return BitConverter.ToInt32(buffer, 0);
    }
}