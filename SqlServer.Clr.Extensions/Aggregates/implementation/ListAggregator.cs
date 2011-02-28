using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal class ListAggregator<T> : IAggregator<T>
    {
        private List<T> _values = new List<T>();

        private readonly Func<IEnumerable<T>, T> _aggregateFunction;
        private static readonly SerializationHelper<T> Helper = SerializationHelper.Create<T>();

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="aggregateFunction">A lambda expression that calculates the aggregated value from a list of values.</param>
        public ListAggregator(Func<IEnumerable<T>, T> aggregateFunction)
        {
            _aggregateFunction = aggregateFunction;
        }

        public void Accumulate(T value, NotUsed parameters)
        {
            _values.Add(value);
        }

        public void Merge(IAggregator value)
        {
            var casted = (ListAggregator<T>) value;
            _values.AddRange(casted._values);
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
                _values.Add(Helper.Read(r));
            }
        }

        public void Write(BinaryWriter w)
        {
            w.Write(_values.Count);
            foreach (var item in _values)
            {
                Helper.Write(w, item);
            }

        }
    }
}