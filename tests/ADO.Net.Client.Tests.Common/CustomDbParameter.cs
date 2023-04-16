using System;
using System.Data;
using System.Data.Common;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbParameter" />
    public class CustomDbParameter : DbParameter
    {
        public override DataRowVersion SourceVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DbType DbType { get; set; }
        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get => throw new System.NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ParameterName { get; set; }
        public override int Size { get;  set; }
        public override string SourceColumn { get => throw new System.NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool SourceColumnNullMapping { get => throw new System.NotImplementedException(); set => throw new NotImplementedException(); }
        public override object Value { get; set; }
        public override byte Precision { get; set; }
        public override byte Scale { get ; set; }

        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }
    }
}