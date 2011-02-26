using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SqlServer.Clr.Extensions.Aggregates
{
    internal static class UserDefinedAggregates
    {
        public static IUserDefinedAggregate<String> StrConcat()
        {
            return new UserDefinedAggregate<string>(
                values => string.Join(",", values.ToArray())
                );
        }

        public static IUserDefinedAggregate<Int64> BitwiseAnd()
        {
            return new UserDefinedAggregate<long>(
                values => values.Aggregate(long.MaxValue, (i,j) => i & j)
                );
        }

        public static IUserDefinedAggregate<Int64> BitwiseOr()
        {
            return new UserDefinedAggregate<long>(
                values => values.Aggregate(long.MaxValue, (i, j) => i | j)
                );
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