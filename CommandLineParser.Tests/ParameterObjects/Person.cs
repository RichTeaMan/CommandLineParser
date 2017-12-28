using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RichTea.CommandLineParser.Tests.ParameterObjects
{
    public class Person
    {
        public string Forename { get; set; }

        public string Surname { get; set; }

        public override string ToString()
        {
            return new ToStringBuilder<Person>(this)
                .Append(Forename)
                .Append(Surname)
                .ToString();
        }

        public override bool Equals(object that)
        {
            var other = that as Person;
            return new EqualsBuilder<Person>(this, that)
                .Append(Forename, other?.Forename)
                .Append(Surname, other?.Surname)
                .Equals();
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder<Person>(this)
                .Append(Forename)
                .Append(Surname)
                .HashCode;
        }
    }
}
