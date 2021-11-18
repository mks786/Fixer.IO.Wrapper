using System;
namespace Fixer.IO.CcyCnvrtr.Config
{
    public class FixerConfig
    {
        public string BaseUri { get; set; }
        public string APIKey { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(BaseUri) &&
                   !string.IsNullOrEmpty(APIKey);
        }
    }
}
