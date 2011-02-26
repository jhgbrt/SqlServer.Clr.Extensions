using System;
using System.Collections.Generic;
using SqlServer.Clr.Extensions.Aggregates.Implementation;
using System.Linq;

namespace SqlServer.Clr.Extensions.Aggregates
{
    /// <summary>
    /// This class contains the definition for all user-defined aggregates.
    /// Provided that certain conditions are met, adding another aggregate function should come down to simply adding
    /// a single function to this class. The function should return an IUserDefinedAggregate[T, TResult] where T is a primitive type
    /// that has an equivalent Sql type (e.g. Int64 corresponds to SqlInt64). 
    /// expression.
    /// </summary>
    internal static class UserDefinedAggregates
    {
        public static IUserDefinedAggregate<Int64, Decimal> Avg()
        {
            return Create<long, decimal>(list => list.Average(i => (decimal)i));
        }

        public static IUserDefinedAggregate<String> StrConcat()
        {
            return Create<string>(s => s, (i,j) => i + "," + j);
        }

        public static IUserDefinedAggregate<Int64> BitwiseAnd()
        {
            return Create(0xFFFFFFFFFFFFFFFL, (i, j) => i & j);
        }

        public static IUserDefinedAggregate<Int64> BitwiseOr()
        {
            return Create(0L, (i, j) => i | j);
        }

        /// <summary>
        /// Creates a user-defined function. Prefer the Create method with a seed and accumulate parameter over this one, since
        /// this implementation needs to collect all values before the value can be aggregated. This implementation is less efficient
        /// as it stores all values in a List before calculating the aggregated value. Needed for aggregations similar to Avg.
        /// </summary>
        /// <typeparam name="T">type to aggregate</typeparam>
        /// <param name="aggregator">aggregation function, calculating the aggregated value from a list of values</param>
        /// <returns></returns>
        private static IUserDefinedAggregate<T, TResult> Create<T, TResult>(Func<IEnumerable<T>, TResult> aggregator)
        {
            return new CollectingAggregationImpl<T, TResult>(aggregator);
        }

        /// <summary>
        /// Create a user-defined aggregation that can be accumulated as values come in. This is the more efficient implementation
        /// and can be used for aggregates similar to Sum, Min, Max (where only one value should be stored to allow calculating the result)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seed">seed value</param>
        /// <param name="accumulate">accumulation function. Should calculate the result based on the current aggregated value and the next value in the list</param>
        /// <returns></returns>
        private static IUserDefinedAggregate<T> Create<T>(T seed, Func<T, T, T> accumulate)
        {
            return new AccumulatingAggregationImpl<T>(seed, accumulate);
        }

        /// <summary>
        /// Create a user-defined aggregation that can be accumulated as values come in. This is the more efficient implementation
        /// and can be used for aggregates similar to Sum, Min, Max (where only one value should be stored to allow calculating the result)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seed">Seeding function. Used for calculating the first value in the list</param>
        /// <param name="accumulate">Accumulation function. Should calculate the result based on the current aggregated value and the next value in the list</param>
        /// <returns></returns>
        private static IUserDefinedAggregate<T> Create<T>(Func<T, T> seed, Func<T, T, T> accumulate)
        {
            return new AccumulatingAggregationImpl<T>(seed, accumulate);
        }
    }
}