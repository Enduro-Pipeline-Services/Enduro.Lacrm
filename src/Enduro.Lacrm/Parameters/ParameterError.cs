using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class ParameterError
    {
        public ParameterError(string parameter, string error)
        {
            Parameter = parameter;
            Error = error;
        }

        public string Parameter { get; }
        public string Error { get; }
    }
}