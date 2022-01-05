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
using System;
using System.Data;
using System.Data.Common;
#endregion

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbParameter" />
    public class CustomDbParameter : DbParameter
    {
        #region Fields/Properties
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
#endregion
        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }
    }
}