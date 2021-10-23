using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using LanguageWire.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LanguageWire.Api.Business
{
    public class TranslatorBusiness : ITranslatorBusiness
    {
        private readonly ILogger<TranslatorBusiness>  _logger;
        private readonly string _pythonPath;

        public TranslatorBusiness(ILogger<TranslatorBusiness> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (configuration != null)
            {
                _pythonPath = configuration?.GetValue<string>("pythonInstallationPath");
            }
        }

        public async Task<string> Translate(string input, string targetLang)
        {
            if (!SupportedLanguages.TargetLanguages.Contains(targetLang))
            {
                _logger.LogWarning("Target language not supported");
                return string.Empty;
            }
            
            string translatedText = String.Empty;
            var builder = new StringBuilder();
            var cleanedInput = input.Replace("\n", "").Replace("\r", "").Trim();
            
            try
            {
                foreach (var word in cleanedInput.Split(' '))
                {
                    var start = new ProcessStartInfo
                    {
                        FileName = _pythonPath,
                        Arguments = string.Format($"Scripts/main.py {word} {targetLang}"),
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    };
                    using var process = Process.Start(start);
                    using var reader = process.StandardOutput;
                    string result = await reader.ReadToEndAsync();
                    builder.Append($" {result}");
                }

                translatedText = builder.Replace("\n", "").Replace("\r", "").ToString().Trim();
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while calling python translator", e.Message);
            }

            return translatedText;
        }
    }
}