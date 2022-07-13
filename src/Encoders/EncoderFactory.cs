namespace dBASE.NET.Encoders;

internal static class EncoderFactory
{
    public static IEncoder GetEncoder(DbfFieldType type)
    {
        return type switch
        {
            DbfFieldType.Character => CharacterEncoder.Instance,
            DbfFieldType.Currency => CurrencyEncoder.Instance,
            DbfFieldType.Date => DateEncoder.Instance,
            DbfFieldType.DateTime => DateTimeEncoder.Instance,
            DbfFieldType.Float => FloatEncoder.Instance,
            DbfFieldType.Integer => IntegerEncoder.Instance,
            DbfFieldType.Logical => LogicalEncoder.Instance,
            DbfFieldType.Memo => MemoEncoder.Instance,
            DbfFieldType.NullFlags => NullFlagsEncoder.Instance,
            DbfFieldType.Numeric => NumericEncoder.Instance,
            _ => throw new ArgumentException("No encoder found for dBASE type " + type)
        };
    }

}