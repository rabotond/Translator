using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            
            var builder = new StringBuilder();
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            var inputWords = input.Split(' ');
            var distinctWords = inputWords.Distinct().ToDictionary(word => word, trans => trans);

            try
            {
                foreach (var word in inputWords.Distinct())
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
                    if (!string.IsNullOrEmpty(result))
                    {
                        distinctWords[word] = result.Replace("\n", "").Replace("\r", "").Trim();
                    }
                }

                foreach (var word in inputWords)
                {
                    builder.Append($" {distinctWords[word]}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while calling python translator", e.Message);
            }

            return regex.Replace(builder.ToString().ToLower().Trim(), " ");
        }
    }
}