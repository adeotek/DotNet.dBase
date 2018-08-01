﻿# dBASE.NET

__dBASE.NET__ is a simple .NET class library used to load dBASE IV .dbf files. The `Dbf` class reads
fields (`DbfField`) and records (`DbfRecord`) from a .dbf file. These fields and records can then
be accessed as lists and looped over.

## Sample of use

### Loading a .dbf file

```c#
using dBASE.NET;
...
Dbf dbf = new Dbf("mydb.dbf");
```

### Looping through fields

```c#
foreach (DbfField field in dbf.Fields)
{
  Console.WriteLine("Field name: " + field.name);
}
```

### Looping through records

```c#
foreach(DbfRecord record in dbf.Records) 
{
  foreach (DbfField fld in dbf.Fields) {
    Console.WriteLine(record.Data[fld]);
  }		  
}
```

## Class diagram

![Class diagram](http://yuml.me/1cc9f823.png)

_yuml:_

```
http://yuml.me/diagram/scruffy/class/edit/[Dbf]+->[DbfRecord], [Dbf]+->[DbfField], [DbfRecord]+->[DbfField], [Dbf]->[DbfHeader], [DbfHeader]^-[Dbf4Header]
```` 
