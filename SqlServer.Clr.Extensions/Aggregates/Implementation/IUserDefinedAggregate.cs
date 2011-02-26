using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal interface IUserDefinedAggregate<T>
    {
        void Accumulate(T value);
        void Merge(IUserDefinedAggregate<T> value);
        T Terminate();
        void Read(BinaryReader binaryReader);
        void Write(BinaryWriter binaryWriter);
    }
}