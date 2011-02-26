﻿using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates
{
    internal class CollectingAggregationImpl<T> : IUserDefinedAggregate<T>
    {
        private List<T> _values = new List<T>();

        private readonly Func<IEnumerable<T>, T> _aggregateFunction;
        private static readonly BinaryHelper<T> Helper = BinaryHelpers.Create<T>();

        /// <summary>
        /// UserDefinedAggregate constructor.
        /// </summary>
        /// <param name="aggregateFunction">A lambda expression that calculates the aggregated value from a list of values.</param>
        public CollectingAggregationImpl(Func<IEnumerable<T>, T> aggregateFunction)
        {
            _aggregateFunction = aggregateFunction;
        }

        public void Accumulate(T value)
        {
            _values.Add(value);
        }

        public void Merge(IUserDefinedAggregate<T> value)
        {
            var casted = (CollectingAggregationImpl<T>) value;
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