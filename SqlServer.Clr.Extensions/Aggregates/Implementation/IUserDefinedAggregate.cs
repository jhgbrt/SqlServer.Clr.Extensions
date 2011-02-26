using Microsoft.SqlServer.Server;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal interface IUserDefinedAggregate<T, TResult> : IBinarySerialize
    {
        void Accumulate(T value);
        void Merge(IUserDefinedAggregate<T, TResult> value);
        TResult Terminate();
    }

    internal interface IUserDefinedAggregate<T> : IUserDefinedAggregate<T,T>
    {
    }
}