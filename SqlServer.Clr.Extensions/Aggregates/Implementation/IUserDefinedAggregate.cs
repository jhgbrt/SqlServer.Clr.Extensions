using Microsoft.SqlServer.Server;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal interface IUserDefinedAggregate<T> : IBinarySerialize
    {
        void Accumulate(T value);
        void Merge(IUserDefinedAggregate<T> value);
        T Terminate();
    }
}