using System.Collections.Generic;

namespace LanguageWire.Api.Models
{
    public static class SupportedLanguages
    {
        public static readonly ISet<string> SourceLanguages = new HashSet<string>() { "en", "fr", "de" };

        public static readonly ISet<string> TargetLanguages = new HashSet<string>() {"en", "fr", "de"};
    }
}