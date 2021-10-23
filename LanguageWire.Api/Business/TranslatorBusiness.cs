using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace LanguageWire.Api.Business
{
    public class TranslatorBusiness : ITranslatorBusiness
    {
        public async Task<string> Translate(string input, string targetLang)
        {
            string translatedText;
            var cleanedInput = input.Replace("\n", "").Replace("\r", "");
            
            try
            {
                var start = new ProcessStartInfo
                {
                    FileName = @"/Library/Frameworks/Python.framework/Versions/3.10/bin/python3",
                    Arguments = string.Format($"Scripts/main.py {cleanedInput} {targetLang}"),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };
                using Process process = Process.Start(start);
                using StreamReader reader = process.StandardOutput;
                string result = reader.ReadToEnd();
                result = result.Replace("\n", "").Replace("\r", "").Trim();

                translatedText = result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return await Task.FromResult(translatedText);
        }
    }
}