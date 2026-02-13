using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class DeleteGroupParams : Parameter
    {
        private string? _groupName;

        public string? GroupId { get; set; }

        public string GroupName
        {
            get => _groupName ?? "";
            set => _groupName = value.Replace(" ", "_");
        }

        public DeleteGroupParams(string groupIdOrName)
        {
            if (groupIdOrName.Contains("_") || !char.IsDigit(groupIdOrName[0]))
            {
                GroupName = groupIdOrName;
            }
            else
            {
                GroupId = groupIdOrName;
            }
            Validators.Add(ValidateGroup);
        }

        public ParameterValidationResponse ValidateGroup()
        {
            return !string.IsNullOrWhiteSpace(GroupId) || !string.IsNullOrWhiteSpace(GroupName)
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(GroupId), "GroupId or GroupName must be provided."));
        }
    }
}
