﻿namespace dBASE.NET.Tests.Encoders
{
    using System;
    using System.Text;

    using dBASE.NET.Encoders;

    using Xunit;

    
    public class DateEncoderTests
    {
        private readonly DbfField dateField = new DbfField("DATADOC", DbfFieldType.Date, 8);

        private readonly Encoding encoding = Encoding.ASCII;

        [Fact]
        public void EncodeTestDateValid()
        {
            // Arrange.
            var val = new DateTime(1939, 09, 01);
            var expectedVal = new[] { '1', '9', '3', '9', '0', '9', '0', '1' };

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = DateEncoder.Instance.Encode(dateField, val, encoding);

            // Assert.
            for (int i = 0; i < dateField.Length; i++)
            {
                AssertX.Equal(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }

        [Fact]
        public void EncodeTestDateNull()
        {
            // Arrange.
            DateTime? val = null;
            string expectedVal = dateField.DefaultValue;

            // Act.
            var expectedEncodedVal = encoding.GetBytes(expectedVal);
            var encodedVal = DateEncoder.Instance.Encode(dateField, val, encoding);

            // Assert.
            for (int i = 0; i < dateField.Length; i++)
            {
                AssertX.Equal(expectedEncodedVal[i], encodedVal[i], $"Position `{i}` failed.");
            }
        }
    }
}