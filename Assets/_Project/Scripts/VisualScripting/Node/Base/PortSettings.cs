namespace Loykas.Scripting
{
    public struct PortSettings
    {
        public bool HideLabel;
        public bool IsConnectionDisabled;
        public bool IsLocalizationDisabled;
        public string LocalizationKey;

        public static PortSettings Default => new()
        {
            HideLabel = false,
            IsConnectionDisabled = false,
            IsLocalizationDisabled = false,
            LocalizationKey = string.Empty,
        };
    }
}
