using System;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;
using SqlServer.Clr.Extensions.Aggregates.Implementation;

namespace SqlServer.Clr.Extensions.Aggregates
{
    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToDuplicates = false,
        IsInvariantToNulls = true,
        IsInvariantToOrder = true,
        MaxByteSize = -1)]
    public class Average : IBinarySerialize
    {
        private IAggregator<Decimal> _impl;

        public Average()
        {
            Init();
        }

        public void Init()
        {
            _impl = UserDefinedAggregates.Average();
        }

        public void Accumulate(SqlDecimal value)
        {
            if (value.IsNull) return;
            _impl.Accumulate(value.Value, new NotUsed());
        }

        public void Merge(Average value)
        {
            _impl.Merge(value._impl);
        }

        public SqlDecimal Terminate()
        {
            return new SqlDecimal(_impl.Terminate());
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
    [SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToDuplicates = false,
        IsInvariantToNulls = true,
        IsInvariantToOrder = false,
        MaxByteSize = -1)]
    public class StrConcat : IBinarySerialize
    {
        private IAggregator<String, String> _impl;

        public StrConcat()
        {
            Init();
        }

        public void Init()
        {
            _impl = UserDefinedAggregates.StrConcat();
        }

        public void Accumulate(SqlString value, SqlString delimiter)
        {
            if (value.IsNull) return;
            _impl.Accumulate(value.Value, delimiter.IsNull ? "," : delimiter.Value);
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
    [SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToDuplicates = true,
        IsInvariantToNulls = true,
        IsInvariantToOrder = true,
        MaxByteSize = -1)]
    public class BitwiseAnd : IBinarySerialize
    {
        private IAggregator<Int64> _impl;

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
            if (value.IsNull) return;
            _impl.Accumulate(value.Value, new NotUsed());
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
    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToDuplicates = true,
        IsInvariantToNulls = true,
        IsInvariantToOrder = true,
        MaxByteSize = -1)]
    public class BitwiseOr : IBinarySerialize
    {
        private IAggregator<Int64> _impl;

        public BitwiseOr()
        {
            Init();
        }

        public void Init()
        {
            _impl = UserDefinedAggregates.BitwiseOr();
        }

        public void Accumulate(SqlInt64 value)
        {
            if (value.IsNull) return;
            _impl.Accumulate(value.Value, new NotUsed());
        }

        public void Merge(BitwiseOr value)
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
}