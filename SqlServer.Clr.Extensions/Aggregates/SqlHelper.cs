using System;

namespace SqlServer.Clr.Extensions.Aggregates
{
    class SqlHelper<TClr, TSql>
    {
        public Func<TClr, TSql> ToSql { get; set; }
        public Func<TSql, TClr> ToClr { get; set; }
    }
}