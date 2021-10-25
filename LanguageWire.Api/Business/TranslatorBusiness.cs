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

        public string Translate(string input, string sourceLang, string targetLang)
        {
            if (!SupportedLanguages.SourceLanguages.Contains(sourceLang) || !SupportedLanguages.TargetLanguages.Contains(targetLang))
            {
                _logger.LogWarning("Language not supported");
                return string.Empty;
            }
            
            var builder = new StringBuilder();
            var regex = new Regex("[ ]{2,}", RegexOptions.None);
            var inputWords = input.Split(' ');
            var distinctWords = inputWords.Distinct().ToDictionary(key => key, value => value);

            try
            {
                Parallel.ForEach(inputWords.Distinct(), word =>
                {
                    var start = new ProcessStartInfo
                    {
                        FileName = _pythonPath,
                        Arguments = string.Format($"Scripts/main.py {word} {sourceLang} {targetLang}"),
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    };
                    using var process = Process.Start(start);
                    using var reader = process.StandardOutput;
                    string result =  reader.ReadToEnd();
                    distinctWords[word] = result;
                });

                foreach (var word in inputWords)
                {
                    builder.Append($" {distinctWords[word]}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while calling python translator", e.Message);
            }

            return regex.Replace(builder.ToString().ToLower().Replace("\n", "").Replace("\r", "").Trim(), " ");
        }
    }
}