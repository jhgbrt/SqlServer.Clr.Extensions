/*using System;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;
using SqlServer.Clr.Extensions.Aggregates;

namespace SqlServer.Clr.Extensions.Aggregates2
{
    [Serializable]
    [SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize = 8000)]
    public class StrConcat : IBinarySerialize
    {
        private IUserDefinedAggregate<string> _impl;

        public StrConcat()
        {
            Init();
        }

        public void Init()
        {
            _impl = UserDefinedAggregates.StrConcat();
        }

        public void Accumulate(SqlString value)
        {
            _impl.Accumulate(value.Value);
        }

        public void Merge(StrConcat value)
        {
            _impl.Merge(value._impl);
        }

        public SqlString Terminate()
        {
            return new SqlString(_impl.Terminate());
        }

        public void Read(BinaryReader r)
        {
            _impl.Read(r);
        }

        public void Write(BinaryWriter w)
        {
            _impl.Write(w);
        }
    }

    [Serializable]
    [SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize = 8000)]
    public class BitwiseAnd : IBinarySerialize
    {
        private IUserDefinedAggregate<long> _impl;

        public BitwiseAnd()
        {
            Init();
        }

        public void Init()
        {
            _impl = UserDefinedAggregates.BitwiseAnd();
        }

        public void Accumulate(SqlInt64 value)
        {
            _impl.Accumulate(value.Value);
        }

        public void Merge(BitwiseAnd value)
        {
            _impl.Merge(value._impl);
        }

        public SqlInt64 Terminate()
        {
            return new SqlInt64(_impl.Terminate());
        }

        public void Read(BinaryReader r)
        {
            _impl.Read(r);
        }

        public void Write(BinaryWriter w)
        {
            _impl.Write(w);
        }
    }
}*/