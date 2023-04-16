using ADO.Net.Client.Annotations;
using System;

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
