namespace dBASE.NET;

/// <summary>
/// The DbfHeader is an abstract base class for headers of different
/// flavors of dBASE files.
/// </summary>
public abstract class DbfHeader
{
    /// <summary>
    /// dBASE version
    /// </summary>
    public DbfVersion Version { get; set; }

    /// <summary>
    /// Date of last update.
    /// </summary>
    public DateTime LastUpdate { get; set;  }

    /// <summary>
    /// Number of records in the file.
    /// </summary>
    public uint NumRecords { get; set; }

    /// <summary>
    ///  Header length in bytes. The records start at this offset in the .dbf file.
    /// </summary>
    public ushort HeaderLength { get; set; }

    /// <summary>
    /// Record length in bytes.
    /// </summary>
    public ushort RecordLength { get; set; }
#pragma warning disable 1591
    public static DbfHeader CreateHeader(DbfVersion version)
    {
        DbfHeader header = version switch
        {
            DbfVersion.FoxBaseDBase3NoMemo => new Dbf3Header(),
            DbfVersion.VisualFoxPro => new Dbf3Header(),
            DbfVersion.VisualFoxProWithAutoIncrement => new Dbf3Header(),
            DbfVersion.FoxPro2WithMemo => new Dbf3Header(),
            DbfVersion.FoxBaseDBase3WithMemo => new Dbf3Header(),
            DbfVersion.dBase4WithMemo => new Dbf3Header(),
            _ => throw new ArgumentException("Unsupported dBASE version: " + version)
        };

        header.Version = version;
        return header;
    }
#pragma warning restore 1591
    /// <summary>
    /// Read the .dbf file header from the specified reader.
    /// </summary>
    internal abstract void Read(BinaryReader reader);

    /// <summary>
    /// Write a .dbf file header to the specified writer.
    /// </summary>
    internal abstract void Write(BinaryWriter writer, List<DbfField> fields, List<DbfRecord> records);
}