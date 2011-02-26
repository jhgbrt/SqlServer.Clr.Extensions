using System;
using SqlServer.Clr.Extensions.Aggregates.Implementation;

namespace SqlServer.Clr.Extensions.Aggregates
{
    /// <summary>
    /// This class contains the definition for all user-defined aggregates.
    /// Provided that certain conditions are met, adding another aggregate function should come down to simply adding
    /// a single function to this class. The function should return an IUserDefinedAggregate[T] where T is a primitive type
    /// that has an equivalent Sql type (e.g. Int64 corresponds to SqlInt64). 
    /// expression.
    /// </summary>
    internal static class UserDefinedAggregates
    {
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

        private static IUserDefinedAggregate<T> Create<T>(T seed, Func<T, T, T> accumulate)
        {
            return new AccumulatingAggregationImpl<T>(seed, accumulate);
        }

        private static IUserDefinedAggregate<T> Create<T>(Func<T, T> seed, Func<T, T, T> accumulate)
        {
            return new AccumulatingAggregationImpl<T>(seed, accumulate);
        }

        public static void test()
        {
            var filename = @"C:\Users\Jeroen\Documents\Visual Studio 2010\Projects\SqlServer.Clr.Extensions\SqlServer.Clr.Extensions\Aggregates\UserDefinedAggregates.cs";

            var text = System.IO.File.ReadAllText(filename);

            var pattern = @"public static IUserDefinedAggregate\< *(?<type>.*) *\> *(?<name>.*)\(\)";

            var matches = System.Text.RegularExpressions.Regex.Matches(text, pattern);

            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                Console.WriteLine(m.Groups["type"].Value);
                Console.WriteLine(m.Groups["name"].Value);
            }

        }
    }
}