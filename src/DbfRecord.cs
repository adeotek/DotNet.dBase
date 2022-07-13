﻿using dBASE.NET.Encoders;
using System.Text;

namespace dBASE.NET;

/// <summary>
/// DbfRecord encapsulates a record in a .dbf file. It contains an array with
/// data (as an Object) for each field.
/// </summary>
public class DbfRecord
{
    private const string DefaultSeparator = ",";
    private const string DefaultMask = "{name}={value}";

    private List<DbfField> fields;

    internal DbfRecord(BinaryReader reader, DbfHeader header, List<DbfField> fields, byte[] memoData, Encoding encoding)
    {
        this.fields = fields;
        Data = new List<object>();

        // Read record marker.
        byte marker = reader.ReadByte();

        // Read entire record as sequence of bytes.
        // Note that record length includes marker.
        byte[] row = reader.ReadBytes(header.RecordLength - 1);
        if (row.Length == 0)
            throw new EndOfStreamException();

        // Read data for each field.
        int offset = 0;
        foreach (DbfField field in fields)
        {
            // Copy bytes from record buffer into field buffer.
            byte[] buffer = new byte[field.Length];
            Array.Copy(row, offset, buffer, 0, field.Length);
            offset += field.Length;

            IEncoder encoder = EncoderFactory.GetEncoder(field.Type);
            Data.Add(encoder.Decode(buffer, memoData, encoding));
        }
    }

    /// <summary>
    /// Create an empty record.
    /// </summary>
    internal DbfRecord(List<DbfField> fields)
    {
        this.fields = fields;
        Data = new List<object>();
        foreach (var unused in fields)
        {
            Data.Add(null);
        }
    }
#pragma warning disable 1591
    public List<object> Data { get; }

    public object this[int index] => Data[index];

    public object this[string name]
    {
        get
        {
            var index = fields.FindIndex(x => x.Name.Equals(name));
            return index == -1 ? null : Data[index];
        }
    }

    public object this[DbfField field]
    {
        get
        {
            var index = fields.IndexOf(field);
            return index == -1 ? null : Data[index];
        }
    }
#pragma warning restore 1591

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return ToString(DefaultSeparator, DefaultMask);
    }

    /// <summary>
    /// Returns a string that represents the current object with custom separator.
    /// </summary>
    /// <param name="separator">Custom separator.</param>
    /// <returns>A string that represents the current object with custom separator.</returns>
    public string ToString(string separator)
    {
        return ToString(separator, DefaultMask);
    }

    /// <summary>
    /// Returns a string that represents the current object with custom separator and mask.
    /// </summary>
    /// <param name="separator">Custom separator.</param>
    /// <param name="mask">
    /// Custom mask.
    /// <para>e.g., "{name}={value}", where {name} is the mask for the field name, and {value} is the mask for the value.</para>
    /// </param>
    /// <returns>A string that represents the current object with custom separator and mask.</returns>
    public string ToString(string separator, string mask)
    {
        separator = separator ?? DefaultSeparator;
        mask = (mask ?? DefaultMask).Replace("{name}", "{0}").Replace("{value}", "{1}");

        return string.Join(separator, fields.Select(z => string.Format(mask, z.Name, this[z])));
    }

    internal void Write(BinaryWriter writer, Encoding encoding)
    {
        // Write marker (always "not deleted")
        writer.Write((byte)0x20);

        var index = 0;
        foreach (var field in fields)
        {
            var encoder = EncoderFactory.GetEncoder(field.Type);
            var buffer = encoder.Encode(field, Data[index], encoding);
            if (buffer.Length > field.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer.Length), buffer.Length, "Buffer length has exceeded length of the field.");
            }

            writer.Write(buffer);
            index++;
        }
    }
}