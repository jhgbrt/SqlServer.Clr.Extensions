using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.SqlServer.Server;

[assembly: CLSCompliant(true)]

namespace SqlServer.Clr.Extensions
{
    public static class Functions
    {

        [SqlFunction(
            DataAccess = DataAccessKind.None, 
            IsDeterministic = true,
            SystemDataAccess = SystemDataAccessKind.None,
            IsPrecise = true,
            FillRowMethodName = "FillInt64",
            TableDefinition = "n bigint")]
        public static IEnumerable Range(SqlInt64 start, SqlInt64 end, SqlInt64 incr)
        {
            for (var i = start.Value; i < end.Value; i += incr.Value) yield return i;
        }

        [SqlFunction(
            DataAccess = DataAccessKind.None,
            IsDeterministic = true,
            SystemDataAccess = SystemDataAccessKind.None,
            IsPrecise = true,
            FillRowMethodName = "FillString",
            TableDefinition = "n nvarchar(max)")]
        public static IEnumerable Split(SqlString input, SqlString separators)
        {
            return input.Value.Split(separators.Value.ToCharArray());
        }

        public static void FillInt64(object row, out SqlInt64 n)
        {
            n = new SqlInt64((long)row);
        }

        public static void FillString(object row, out SqlString n)
        {
            n = new SqlString((string)row);
        }
    }
}
