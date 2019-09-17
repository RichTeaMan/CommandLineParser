using RichTea.Common;
using System.Collections.Generic;

namespace RichTea.CommandLineParser.Tests
{
    public class MethodInvocationInfo
    {
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        public string MethodName { get; set; }

        public MethodInvocationInfo AddParameter(string parameterName, object value)
        {
            Parameters.Add(parameterName, value);
            return this;
        }

        public override string ToString()
        {
            return new ToStringBuilder<MethodInvocationInfo>(this)
                .Append(p => p.MethodName)
                .Append(p => p.Parameters)
                .ToString();
        }

        public override bool Equals(object that)
        {
            return new EqualsBuilder<MethodInvocationInfo>(this, that)
                .Append(p => p.MethodName)
                .Append(p => p.Parameters)
                .AreEqual;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(MethodName)
                .Append(Parameters)
                .HashCode;
        }
    }
}
