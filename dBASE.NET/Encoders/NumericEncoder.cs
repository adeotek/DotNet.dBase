﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dBASE.NET.Encoders
{
	internal class NumericEncoder: IEncoder
	{
		private static NumericEncoder instance = null;

		private NumericEncoder() { }

		public static NumericEncoder Instance
		{
			get
			{
				if (instance == null) instance = new NumericEncoder();
				return instance;
			}
		}

		public byte[] Encode(DbfField field, object data)
		{
			string text = Convert.ToString(data).PadLeft(field.Length, ' ');
			if (text.Length > field.Length) text.Substring(0, field.Length);
			return Encoding.ASCII.GetBytes(text);
		}
	}
}