namespace dBASE.NET;
#pragma warning disable 1591
public class Dbf3Header : DbfHeader
#pragma warning restore 1591
{
    internal override void Read(BinaryReader reader)
    {
        Version = (DbfVersion) reader.ReadByte();
        var year = 1900 + reader.ReadByte();
        var month = reader.ReadByte();
        var day = reader.ReadByte();
        LastUpdate = new DateTime(year, month < 1 ? 1 : month, day < 1 ? 1 : day);
        NumRecords = reader.ReadUInt32();
        HeaderLength = reader.ReadUInt16();
        RecordLength = reader.ReadUInt16();
        reader.ReadBytes(20); // Skip rest of header.
    }

    internal override void Write(BinaryWriter writer, List<DbfField> fields, List<DbfRecord> records)
    {
        LastUpdate = DateTime.Now;
        // Header length = header fields (32b ytes)
        //               + 32 bytes for each field
        //               + field descriptor array terminator (1 byte)
        HeaderLength = (ushort)(32 + fields.Count * 32 + 1);
        NumRecords = (uint)records.Count;
        RecordLength = 1;
        foreach (var field in fields)
        {
            RecordLength += field.Length;
        }

        writer.Write((byte)Version);
        writer.Write((byte)(LastUpdate.Year - 1900));
        writer.Write((byte)(LastUpdate.Month));
        writer.Write((byte)(LastUpdate.Day));
        writer.Write(NumRecords);
        writer.Write(HeaderLength);
        writer.Write(RecordLength);
        for (var i = 0; i < 20; i++)
        {
            writer.Write((byte)0);
        }
    }
}