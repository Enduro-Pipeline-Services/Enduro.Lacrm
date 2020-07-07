using Enduro.Lacrm.Parameters;

namespace Enduro.Lacrm.Functions
{
    public interface ILacrmFunction<TParams> where TParams : Parameter
    {
        public string Function { get; }
        public TParams Parameters { get; }
    }
}