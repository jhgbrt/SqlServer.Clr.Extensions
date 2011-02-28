using System;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    /// <summary>
    /// Implementation that calculates the aggregate value 'on the fly'. Can be used for aggregations where
    /// the complete list of values is not needed to calculate the end result (e.g. sum, min, max). 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class AccumulatingAggregator<T> : IAggregator<T>
    {
        private T _aggregatedValue;

        private Func<T, T> _seed;
        private readonly Func<T, T, T> _accumulate;
        private static readonly SerializationHelper<T> Helper = SerializationHelper.Create<T>();

        public AccumulatingAggregator(
            T seed,
            Func<T, T, T> accumulate)
        {
            _aggregatedValue = seed;
            _accumulate = accumulate;
        }

        public AccumulatingAggregator(
            Func<T,T> seed,
            Func<T, T, T> accumulate)
        {
            _seed = seed;
            _accumulate = accumulate;
        }

        public void Accumulate(T value, NotUsed parameters)
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

        public void Merge(IAggregator value)
        {
            var casted = (AccumulatingAggregator<T>) value;
            _aggregatedValue = _accumulate(_aggregatedValue, casted._aggregatedValue);
        }

        public T Terminate()
        {
            return _aggregatedValue;
        }

        public void Read(BinaryReader binaryReader)
        {
            _aggregatedValue = Helper.Read(binaryReader);
        }

        public void Write(BinaryWriter binaryWriter)
        {
            Helper.Write(binaryWriter, _aggregatedValue);
        }
    }
}