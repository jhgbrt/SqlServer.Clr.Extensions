using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates
{
    static class BinaryHelpers
    {
        private static readonly IDictionary<Type, object > Helpers = new Dictionary<Type, object>();

        static BinaryHelpers()
        {
            Helpers[typeof(string)] = String;
            Helpers[typeof(long)] = Int64;
        }

        private static BinaryHelper<Int64> Int64
        {
            get { return new BinaryHelper<long> { BinaryRead = r => r.ReadInt64(), BinaryWrite = (w, v) => w.Write(v) }; }
        }

        private static BinaryHelper<String> String
        {
            get { return new BinaryHelper<string> { BinaryRead = r => r.ReadString(), BinaryWrite = (w, v) => w.Write(v) }; }
        }

        public static BinaryHelper<T> Create<T>()
        {
            if (!Helpers.ContainsKey(typeof(T)))
            {
                var methodName = string.Format("Read{0}", typeof (T).Name);
                var readerMethod = typeof (BinaryReader).GetMethod(methodName);
                if (readerMethod == null) throw new NotSupportedException(string.Format("Can not create a BinaryHelper<{0}> because BinaryReader does not have a method called {1}", typeof(T).Name, methodName));
                var writerMethod = typeof (BinaryWriter).GetMethod("Write", new[] {typeof (T)});
                var readerDelegate = Delegate.CreateDelegate(typeof(Func<BinaryReader, T>), readerMethod);
                var writerDelegate = Delegate.CreateDelegate(typeof(Action<BinaryWriter, T>), writerMethod);
                var helper = new BinaryHelper<T>
                                 {
                                     BinaryRead = (Func<BinaryReader, T>) readerDelegate,
                                     BinaryWrite = (Action<BinaryWriter, T>) writerDelegate
                                 };
                Helpers[typeof (T)] = helper;
            }
            return (BinaryHelper<T>) Helpers[typeof (T)];
        }
    }
}