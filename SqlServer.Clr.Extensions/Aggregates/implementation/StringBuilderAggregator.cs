using System.IO;
using System.Text;

namespace SqlServer.Clr.Extensions.Aggregates.Implementation
{
    internal class StringBuilderAggregator : IAggregator<string, string>
    {
        private StringBuilder _accumulator = new StringBuilder();
        private static readonly SerializationHelper<string> Helper = SerializationHelper.Create<string>();
        private string _delimiter;

        public void Read(BinaryReader r)
        {
            _accumulator = new StringBuilder(Helper.Read(r));
        }

        public void Write(BinaryWriter w)
        {
            Helper.Write(w, _accumulator.ToString());
        }

        public void Accumulate(string value, string delimiter)
        {
            _delimiter = delimiter;
            AppendDelimiter();
            _accumulator.Append(value);
        }

        private void AppendDelimiter()
        {
            if (_accumulator.Length > 0) _accumulator.Append(_delimiter);
        }

        public void Merge(IAggregator value)
        {
            var other = (StringBuilderAggregator) value;
            AppendDelimiter();
            _accumulator.Append(other._accumulator.ToString());
        }

        public string Terminate()
        {
            return _accumulator.ToString();
        }
    }
}