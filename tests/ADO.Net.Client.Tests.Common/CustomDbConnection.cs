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
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbConnection" />
    public class CustomDbConnection : DbConnection
    {
        private ConnectionState _state = ConnectionState.Open;
        protected override DbProviderFactory DbProviderFactory => CustomDbProviderFactory.Instance;
        public override string ConnectionString { get; set; }

        public override string Database => throw new System.NotImplementedException();

        public override string DataSource => throw new System.NotImplementedException();

        public override string ServerVersion => throw new System.NotImplementedException();

        public override ConnectionState State => _state;

        public override void ChangeDatabase(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }

        public override void Open()
        {
            throw new System.NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new CustomDbTransaction(this, isolationLevel);
        }
#if !NET461 && !NETSTANDARD2_0
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override ValueTask<DbTransaction> BeginDbTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            return new ValueTask<DbTransaction>(new CustomDbTransaction(this, isolationLevel));
        }
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override DbCommand CreateDbCommand()
        {
            return new CustomDbCommand();
        }
        #region Constructors
        public CustomDbConnection()
        {

        }
        public CustomDbConnection(ConnectionState state)
        {
            _state = state;
        }
        #endregion
    }
}