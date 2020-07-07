using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetPipelineSettings : ILacrmFunction<GetPipelineSettingsParams>
    {
        public GetPipelineSettings()
        {
            Parameters = new GetPipelineSettingsParams();
        }
        
        public GetPipelineSettings(GetPipelineSettingsParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetPipelineSettings";
        public GetPipelineSettingsParams Parameters { get; }
    }
}