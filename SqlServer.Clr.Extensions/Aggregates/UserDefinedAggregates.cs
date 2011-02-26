using System;
using System.Collections.Generic;
using System.Linq;
using SqlServer.Clr.Extensions.Aggregates.Implementation;

namespace SqlServer.Clr.Extensions.Aggregates
{
    /// <summary>
    /// This class contains the definition for all user-defined aggregates.
    /// Provided that certain conditions are met, adding another aggregate function should come down to simply adding
    /// a single function to this class. The function should return an IUserDefinedAggregate[T] where T is a primitive type
    /// that has an equivalent Sql type (e.g. Int64 corresponds to SqlInt64). 
    /// You need to use the 'clr notation' (Int64 instead of long) in order to make the Aggregates.tt file work.
    /// </summary>
    internal static class UserDefinedAggregates
    {
        // TODO constructing a SqlDecimal from Decimal seems to loose precision, therefore aggregates working on lists of decimal do not yet work
        public static IUserDefinedAggregate<Decimal> Average()
        {
            return Create<decimal>(list => list.Average());
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

        private static IUserDefinedAggregate<T> Create<T>(Func<IEnumerable<T>, T> aggregate)
        {
            return new CollectingAggregationImpl<T>(aggregate);
        }

        private static IUserDefinedAggregate<T> Create<T>(T seed, Func<T, T, T> accumulate)
        {
            return new AccumulatingAggregationImpl<T>(seed, accumulate);
        }

        private static IUserDefinedAggregate<T> Create<T>(Func<T, T> seed, Func<T, T, T> accumulate)
        {
            return new AccumulatingAggregationImpl<T>(seed, accumulate);
        }
    }
}