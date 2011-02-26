using System;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates
{
    class BinaryHelper<TClr>
    {
        public Func<BinaryReader, TClr> BinaryRead { get; set; }
        public Action<BinaryWriter, TClr> BinaryWrite { get; set; }
    }
}