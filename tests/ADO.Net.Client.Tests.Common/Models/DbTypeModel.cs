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
#if NET6_0_OR_GREATER
        /// <summary>
        /// 
        /// </summary>
        public DateOnly Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeOnly Time { get; set; }
#endif
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TimeSpan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime NormalDateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DateTime2]
        public DateTime DateTime2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NormalString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ANSIString]
        public string AnsiString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [ANSIStringFixedLength]
        public string AnsiStringFixedLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringFixedLength]
        public string StringFixedLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset DateTimeOffset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] ByteArray { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float Float { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Bool { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte Byte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Decimal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Double { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public sbyte SByte { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public short Short { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Int { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Long { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ushort UShort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint UInt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ulong ULong { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Object { get; set; }
    }
}
