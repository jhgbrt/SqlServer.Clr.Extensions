using System;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates
{
    /// <summary>
    /// Implementation that calculates the aggregate value 'on the fly'. Can be used for aggregations where
    /// the complete list of values is not needed to calculate the end result (e.g. sum, min, max). 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class AccumulatingAggregationImpl<T> : IUserDefinedAggregate<T>
    {
        private T _aggregatedValue;

        private Func<T, T> _seed;
        private readonly Func<T, T, T> _accumulate;
        private static readonly BinaryHelper<T> Helper = BinaryHelpers.Create<T>();

        public AccumulatingAggregationImpl(
            T seed,
            Func<T, T, T> accumulate)
        {
            _aggregatedValue = seed;
            _accumulate = accumulate;
        }

        public AccumulatingAggregationImpl(
            Func<T,T> seed,
            Func<T, T, T> accumulate)
        {
            _seed = seed;
            _accumulate = accumulate;
        }

        public void Accumulate(T value)
        {
            if (_seed != null)
            {
                _aggregatedValue = _seed(value);
                _seed = null;
            }
            else
            {
                _aggregatedValue = _accumulate(_aggregatedValue, value);
            }
        }

        public void Merge(IUserDefinedAggregate<T> value)
        {
            var casted = (AccumulatingAggregationImpl<T>) value;
            _aggregatedValue = _accumulate(_aggregatedValue, casted._aggregatedValue);
        }

        public T Terminate()
        {
            return _aggregatedValue;
        }

        public void Read(BinaryReader binaryReader)
        {
            _aggregatedValue = Helper.BinaryRead(binaryReader);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            Helper.BinaryWrite(binaryWriter, _aggregatedValue);
        }
    }
}