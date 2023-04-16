using System;
using System.Collections.Generic;
using System.Data;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IDataRecord" />
    public class CustomDataRecord : IDataRecord
    {
        private readonly List<KeyValuePair<string, object>> _kvp;
        public object this[int i] => GetValue(i);

        public object this[string name] => GetOrdinal(name);

        public int FieldCount => _kvp.Count;
  
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDataRecord"/> class.
        /// </summary>
        /// <param name="kvp">The KVP.</param>
        public CustomDataRecord(List<KeyValuePair<string, object>> kvp)
        {
            _kvp = kvp;
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public virtual string GetName(int i)
        {
            return _kvp[i].Key;
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public virtual object GetValue(int i)
        {
            return _kvp[i].Value;
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return (_kvp[i].Value == DBNull.Value || _kvp[i].Value == null);
        }
    }
}
