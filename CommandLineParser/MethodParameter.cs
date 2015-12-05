using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineParser
{
    public abstract class MethodParameter<T> : MethodParameter
    {
        public new virtual T DefaultValue { get { return (T)base.DefaultValue; } }

        protected object BaseDefaultValue { get { return base.DefaultValue; } set { base.DefaultValue = value; } }

        public bool IsNullable { get; private set; }

        public MethodParameter(ParameterInfo parameterInfo)
            : base(parameterInfo)
        {
            if (parameterInfo.ParameterType != typeof(T))
            {
                if (Nullable.GetUnderlyingType(parameterInfo.ParameterType) == typeof(T))
                {
                    IsNullable = true;
                }
                else if((parameterInfo.ParameterType.IsArray &&
                    parameterInfo.ParameterType.GetElementType() == typeof(T)) == false)
                {
                    throw new ArgumentException(
                        string.Format(
                            "Parameter '{0}' is not the correct type for '{1}'.",
                            parameterInfo.Name,
                            this));
                }
            }
        }

        public abstract T GetTypedParameter(string argument);

        public T[] GetTypedParameterArray(string[] arguments)
        {
            var param = new List<T>();
            foreach (var a in arguments)
            {
                var p = GetTypedParameter(a);
                param.Add(p);
            }
            return param.ToArray();
        }

        public override object GetParameter(string argument)
        {
            return GetTypedParameter(argument);
        }

        public override object GetParameterArray(string[] arguments)
        {
            return GetTypedParameterArray(arguments);
        }
    }

    public abstract class MethodParameter
    {
        public string Name { get; protected set; }
        public string[] Aliases { get; protected set; }
        public virtual object DefaultValue { get; protected set; }
        public bool Optional { get; protected set; }
        public int Position { get; private set; }
        public bool IsArray { get; private set; }

        public MethodParameter(ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name;
            var attr = parameterInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(ClArgsAttribute));
            var arg = attr as ClArgsAttribute;
            if (arg == null)
                throw new ArgumentException("The parameter does not have a ClArgs Attribute.");
            Aliases = arg.Args;
            Optional = parameterInfo.HasDefaultValue;
            Position = parameterInfo.Position;
            IsArray = parameterInfo.ParameterType.IsArray;
        }

        public bool SupportsArgument(IEnumerable<string> parameters)
        {
            var aliases = parameters.Where(p => Aliases.Contains(p));
            // Ensure there are no multiple aliases that could create ambiguity
            return aliases.Count() == 1 || (aliases.Count() == 0 && Optional);
        }

        public string GetSupportedAlias(IEnumerable<string> parameters)
        {
            var aliases = parameters.Where(p => Aliases.Contains(p));

            if (aliases.Count() == 1)
                return aliases.Single();
            return null;
        }

        public abstract object GetParameter(string argument);

        public virtual object GetParameterArray(string[] arguments)
        {
            var param = new List<object>();
            foreach (var a in arguments)
            {
                var p = GetParameter(a);
                param.Add(p);
            }
            return param.ToArray();
        }
    }
}
