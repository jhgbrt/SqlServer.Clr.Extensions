using System;
using System.Collections.Generic;
using System.IO;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    class SerializationHelper<TClr>
    {
        public Func<BinaryReader, TClr> Read { get; set; }
        public Action<BinaryWriter, TClr> Write { get; set; }
    }

    internal static class SerializationHelper
    {
        private static readonly IDictionary<Type, object> Helpers = new Dictionary<Type, object>();

        public static SerializationHelper<T> Create<T>()
        {
            if (!Helpers.ContainsKey(typeof(T)))
            {
                var methodName = string.Format("Read{0}", typeof(T).Name);
                var readerMethod = typeof(BinaryReader).GetMethod(methodName);
                if (readerMethod == null) throw new NotSupportedException(string.Format("Can not create a SerializationHelper<{0}> because BinaryReader does not have a method called {1}", typeof(T).Name, methodName));
                var writerMethod = typeof(BinaryWriter).GetMethod("Write", new[] { typeof(T) });
                var readerDelegate = Delegate.CreateDelegate(typeof(Func<BinaryReader, T>), readerMethod);
                var writerDelegate = Delegate.CreateDelegate(typeof(Action<BinaryWriter, T>), writerMethod);
                var helper = new SerializationHelper<T>
                {
                    Read = (Func<BinaryReader, T>)readerDelegate,
                    Write = (Action<BinaryWriter, T>)writerDelegate
                };
                Helpers[typeof(T)] = helper;
            }
            return (SerializationHelper<T>)Helpers[typeof(T)];
        }
    }
}