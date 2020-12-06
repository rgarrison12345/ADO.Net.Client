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
using System.Data.Common;
#if NET40 || NET45 || NET461
using System.Security;
using System.Security.Permissions;
#endif
#endregion

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbProviderFactory" />
    public class CustomDbProviderFactory : DbProviderFactory
    {
        #region Fields/Properties
        public static CustomDbProviderFactory Instance = new CustomDbProviderFactory();

#if ADVANCED_FEATURES
        public override bool CanCreateCommandBuilder => base.CanCreateCommandBuilder;
        public override bool CanCreateDataAdapter => base.CanCreateDataAdapter;
#endif
        #endregion
        #region Constructors
        private CustomDbProviderFactory()
        {

        }
        #endregion
        #region Utility Methods
        public override DbCommandBuilder CreateCommandBuilder()
        {
            return new CustomDbCommandBuilder();
        }
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new CustomConnectionStringBuilder();
        }
        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return new CustomDbDataSourceEnumerator();
        }
        public override DbDataAdapter CreateDataAdapter()
        {
            return new CustomDataAdapter();
        }
        public override DbConnection CreateConnection()
        {
            return new CustomDbConnection();
        }
        public override DbCommand CreateCommand()
        {
            return new CustomDbCommand();
        }
        public override DbParameter CreateParameter()
        {
            return new CustomDbParameter();
        }
#if NET40 || NET45 || NET461
        public override CodeAccessPermission CreatePermission(PermissionState state)
        {
            return new CustomDbCodePermission();
        }
#endif
        #endregion
    }
}
