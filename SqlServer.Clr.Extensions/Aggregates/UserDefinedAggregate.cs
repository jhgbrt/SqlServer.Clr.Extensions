using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates
{
    internal class UserDefinedAggregate<T> : IUserDefinedAggregate<T>
    {
        private List<T> _values = new List<T>();

        private readonly Func<IEnumerable<T>, T> _aggregateFunction;
        private static readonly BinaryHelper<T> Helper = BinaryHelpers.Create<T>();

        public UserDefinedAggregate(Func<IEnumerable<T>, T> aggregateFunction)
        {
            _aggregateFunction = aggregateFunction;
        }

        public void Accumulate(T value)
        {
            _values.Add(value);
        }

        public void Merge(IUserDefinedAggregate<T> value)
        {
            _values.AddRange(value.Values);
        }

        public IEnumerable<T> Values
        {
            get { return _values; }
        }

        public T Terminate()
        {
            return _aggregateFunction(_values);
        }

        public void Read(BinaryReader r)
        {
            int itemCount = r.ReadInt32();
            _values = new List<T>(itemCount);
            for (int i = 0; i <= itemCount - 1; i++)
            {
                _values.Add(Helper.BinaryRead(r));
            }
        }

        public void Write(BinaryWriter w)
        {
            w.Write(_values.Count);
            foreach (var item in _values)
            {
                Helper.BinaryWrite(w, item);
            }

        }
    }
}