#region Licenses
/*MIT License
Copyright(c) 2020
Robert Garrison

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion
#region Using Statements
using ADO.Net.Client.Annotations;
using System;
#endregion

namespace ADO.Net.Client.Tests.Common.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DbTypeModel
    {
        public TimeSpan Time { get; set; }
        public DateTime NormalDateTime { get; set; }
        [DateTime2]
        public DateTime DateTime2 { get; set; }
        public string NormalString { get; set; }
        [ANSIString]
        public string AnsiString { get; set; }
        [ANSIStringFixedLength]
        public string AnsiStringFixedLength { get; set; }
        [StringFixedLength]
        public string StringFixedLength { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public byte[] ByteArray { get; set; }
        public float Float { get; set; }
        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public decimal Decimal { get; set; }
        public double Double { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public ushort UShort { get; set; }
        public uint UInt { get; set; }
        public ulong ULong { get; set; }
        public Guid Guid { get; set; }
        public object Object { get; set; }
    }
}
