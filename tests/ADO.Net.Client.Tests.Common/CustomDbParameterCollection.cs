using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace ADO.Net.Client.Tests.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DbParameterCollection" />
    public class CustomDbParameterCollection : DbParameterCollection
    {
        private readonly List<CustomDbParameter> paramList = new List<CustomDbParameter>();
        public override int Count => paramList.Count;

        public override object SyncRoot => throw new NotImplementedException();

        public override bool IsFixedSize => true;
        public override bool IsReadOnly => true;
        public override bool IsSynchronized => true;
        public override int Add(object value)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(Array values)
        {
            foreach(CustomDbParameter param in values)
            {
                paramList.Add(param);
            }
        }
        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            return paramList.GetEnumerator();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            return paramList[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return paramList.Where(x => x.ParameterName == parameterName).SingleOrDefault();
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            throw new NotImplementedException();
        }
    }
}