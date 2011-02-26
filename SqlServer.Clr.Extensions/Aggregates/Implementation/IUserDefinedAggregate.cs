using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal interface IUserDefinedAggregate<T, TResult>
    {
        void Accumulate(T value);
        void Merge(IUserDefinedAggregate<T, TResult> value);
        TResult Terminate();
        void Read(BinaryReader binaryReader);
        void Write(BinaryWriter binaryWriter);
    }

    internal interface IUserDefinedAggregate<T> : IUserDefinedAggregate<T,T>
    {
        
    }
}