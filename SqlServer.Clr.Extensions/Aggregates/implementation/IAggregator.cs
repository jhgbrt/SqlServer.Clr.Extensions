using Microsoft.SqlServer.Server;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal interface IAggregator : IBinarySerialize { }

    internal interface IAggregator<T, in TParam> : IAggregator
    {
        void Accumulate(T value, TParam parameters);
        void Merge(IAggregator value);
        T Terminate();
    }

    class NotUsed{}

    internal interface IAggregator<T> : IAggregator<T,NotUsed>
    {        
    }
}