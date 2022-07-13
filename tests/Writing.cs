﻿using dBASE.NET;
using System;
using System.IO;
using Xunit;

namespace dBASE.NET.Tests;

public class Writing
{
    private Dbf dbf;

    private byte[] ReadBytes()
    {
        // Open stream for reading.
        var stream = File.Open("test.dbf", FileMode.Open, FileAccess.Read);
        var reader = new BinaryReader(stream);
        var data = reader.ReadBytes((int) stream.Length);
        reader.Close();
        stream.Close();
        return data;
    }

    [Fact]
    public void WriteNoData()
    {
        dbf = new Dbf();
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        var data = ReadBytes();
        AssertX.Equal(0x30, data[0], "Version should be 0x30.");
    }

    [Fact]
    public void WriteOneField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
        dbf.Fields.Add(field);
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal("TEST", dbf.Fields[0].Name, "Field name should be TEST.");
    }

    [Fact]
    public void WriteFieldAndRecord()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = "HELLO";
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal("HELLO", dbf.Records[0][0], "Record content should be HELLO.");
    }

    [Fact]
    public void WriteFieldAndThreeRecords()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Character, 12);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = "HELLO";
        record = dbf.CreateRecord();
        record.Data[0] = "WORLD";
        record = dbf.CreateRecord();
        record.Data[0] = "OUT THERE";
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal("OUT THERE", dbf.Records[2][0], "Record content should be OUT THERE.");
    }

    [Fact]
    public void NumericField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Numeric, 12, 2);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = 3.14;
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal(3.14, dbf.Records[0][0], "Record content should be 3.14.");
    }

    [Fact]
    public void FloatField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Float, 12);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = 3.14f;
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal(3.14f, dbf.Records[0][0], "Record content should be 3.14.");
    }

    [Fact]
    public void LogicalField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Logical, 12);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = true;
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal(true, dbf.Records[0][0], "Record content should be TRUE.");
    }

    [Fact]
    public void DateField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Date, 12);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = new DateTime(2018, 8, 7);
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal(new DateTime(2018, 8, 7), dbf.Records[0][0], "Record content should be 2018-08-07.");
    }

    [Fact]
    public void DateTimeField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.DateTime, 8);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = new DateTime(2018, 8, 7, 20, 15, 8);
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal(new DateTime(2018, 8, 7, 20, 15, 8), dbf.Records[0][0], "Record content should be 2018-08-07 20:15:08.");
    }

    [Fact]
    public void IntegerField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Integer, 4);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = 34;
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal(34, dbf.Records[0][0], "Record content should be 34.");
    }

    [Fact]
    public void CurrencyField()
    {
        dbf = new Dbf();
        DbfField field = new DbfField("TEST", DbfFieldType.Currency, 4);
        dbf.Fields.Add(field);
        DbfRecord record = dbf.CreateRecord();
        record.Data[0] = 4.34F;
        dbf.Write("test.dbf", DbfVersion.VisualFoxPro);

        dbf = new Dbf();
        dbf.Read("test.dbf");

        AssertX.Equal((float) 4.34, dbf.Records[0][0], "Record content should be 4.34.");
    }
}