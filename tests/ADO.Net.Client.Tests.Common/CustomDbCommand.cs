using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Tests.Common
{
    public class CustomDbCommand : DbCommand
    {
        private readonly CustomDbParameterCollection collection = new CustomDbParameterCollection();
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection => collection;

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
            
        }

        public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            return await base.ExecuteNonQueryAsync(cancellationToken);
        }
        public override int ExecuteNonQuery()
        {
            return 1;
        }
        public override object ExecuteScalar()
        {
            return "Some String";
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new CustomDbReader());
        }
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new CustomDbReader();
        }
    }
}