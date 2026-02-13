using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class CreateGroupParams : Parameter
    {
        private string? _groupName;

        public string GroupName
        {
            get => _groupName ?? "";
            set => _groupName = value.Replace(" ", "_");
        }

        public CreateGroupParams(string groupName)
        {
            GroupName = groupName;
            Validators.Add(ValidateGroupName);
        }

        public ParameterValidationResponse ValidateGroupName()
        {
            return !string.IsNullOrWhiteSpace(GroupName)
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(GroupName), "GroupName cannot be null or empty."));
        }
    }
}
