using System;
using System.Collections.Concurrent;
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
        private const int maxDegreeOfParallelism = 20;
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
            if (!SupportedLanguages.SourceLanguages.Contains(sourceLang))
            {
                _logger.LogWarning("Source langauge not supported, source value: {SourceLang} ", sourceLang);
                return string.Empty;
            }

            if(!SupportedLanguages.TargetLanguages.Contains(targetLang))
            {
                _logger.LogWarning("Target language not supported, target value: {TargetLang}", targetLang);
                return string.Empty;
            }
            
            var inputWords = input.Split(' ');
            var concurrentDictionary = new ConcurrentDictionary<string, string>();

            try
            {
                Parallel.ForEach(inputWords.Distinct(), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, word =>
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

                    concurrentDictionary[word] = result;
                });

                var builder = new StringBuilder();

                foreach (var word in inputWords)
                {
                    builder.Append($" {concurrentDictionary[word]}");
                }

                var regex = new Regex("[ ]{2,}", RegexOptions.None);

                // replacing special characters to sanitize python output and remove multiple whitespaces
                return regex.Replace(builder.ToString().ToLower().Replace("\n", "").Replace("\r", "").Trim(), " ");
            }
            catch (Exception e)
            {
                _logger.LogError("Error occured while calling python translator", e.Message);
                return string.Empty;
            }
        }
    }
}