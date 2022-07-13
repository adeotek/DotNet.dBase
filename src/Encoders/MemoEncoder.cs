using System.Text;

namespace dBASE.NET.Encoders;

internal class MemoEncoder : IEncoder
{
    private static MemoEncoder _instance;

    private MemoEncoder() { }

    public static MemoEncoder Instance => _instance ??= new MemoEncoder();

    /// <inheritdoc />
    public byte[] Encode(DbfField field, object data, Encoding encoding)
    {
        return null;
    }

    /// <inheritdoc />
    public object Decode(byte[] buffer, byte[] memoData, Encoding encoding)
    {
        var index = 0;
        // Memo fields of 5+ byts in length store their index in text, e.g. "     39394"
        // Memo fields of 4 bytes store their index as an int.
        if (buffer.Length > 4)
        {
            var text = encoding.GetString(buffer).Trim();
            if (text.Length == 0) return null;
            index = Convert.ToInt32(text);
        }
        else
        {
            index = BitConverter.ToInt32(buffer, 0);
            if (index == 0) return null;
        }

        return FindMemo(index, memoData, encoding);
    }

    private static string FindMemo(int index, byte[] memoData, Encoding encoding)
    {
        // This is the original implementation of findMemo. It was found that
        // the LINQ methods are orders of magnitude slower than using using array
        // offsets.

        /* UInt16 blockSize = BitConverter.ToUInt16(memoData.Skip(6).Take(2).Reverse().ToArray(), 0);
           int type = (int)BitConverter.ToUInt32(memoData.Skip(index * blockSize).Take(4).Reverse().ToArray(), 0);
           int length = (int)BitConverter.ToUInt32(memoData.Skip(index * blockSize + 4).Take(4).Reverse().ToArray(), 0);
           string text = encoding.GetString(memoData.Skip(index * blockSize + 8).Take(length).ToArray()).Trim();
           return text; */

        // The index is measured from the start of the file, even though the memo file header blocks takes
        // up the first few index positions.
        var blockSize = BitConverter.ToUInt16(new[] { memoData[7], memoData[6] }, 0);
        var length = (int)BitConverter.ToUInt32(
            new[]
            {
                memoData[index * blockSize + 4 + 3],
                memoData[index * blockSize + 4 + 2],
                memoData[index * blockSize + 4 + 1],
                memoData[index * blockSize + 4 + 0],
            },
            0);

        var memoBytes = new byte[length];
        var lengthToSkip = index * blockSize + 8;

        for (var i = lengthToSkip; i < lengthToSkip + length; ++i)
        {
            memoBytes[i - lengthToSkip] = memoData[i];
        }

        return encoding.GetString(memoBytes).Trim();
    }
}