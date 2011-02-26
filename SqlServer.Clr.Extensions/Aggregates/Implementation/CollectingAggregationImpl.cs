using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal class CollectingAggregationImpl<T, TResult> : IUserDefinedAggregate<T, TResult>
    {
        private List<T> _values = new List<T>();
        private readonly Func<IEnumerable<T>, TResult> _aggregateFunction;
        private static readonly SerializationHelper<T> Helper = SerializationHelper.Create<T>();

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="aggregateFunction">A lambda expression that calculates the aggregated value from a list of values.</param>
        public CollectingAggregationImpl(Func<IEnumerable<T>, TResult> aggregateFunction)
        {
            _aggregateFunction = aggregateFunction;
        }

        public void Accumulate(T value)
        {
            _values.Add(value);
        }

        public void Merge(IUserDefinedAggregate<T, TResult> value)
        {
            var casted = (CollectingAggregationImpl<T, TResult>) value;
            _values.AddRange(casted._values);
        }

        public TResult Terminate()
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