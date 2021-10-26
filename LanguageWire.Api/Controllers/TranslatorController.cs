using System;
using System.Net.Mime;
using LanguageWire.Api.Business;
using Microsoft.AspNetCore.Mvc;

namespace LanguageWire.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TranslatorController : ControllerBase
    {
        private readonly ITranslatorBusiness _translatorBusiness;
        
        public TranslatorController(ITranslatorBusiness translatorBusiness)
        {
            _translatorBusiness = translatorBusiness ?? throw new ArgumentNullException(nameof(translatorBusiness));
        }

        /// <summary>
        /// Translates a text from a source langauge to a target language
        /// </summary>
        /// <param name="input">Input text to be translated</param>
        /// <param name="sourceLanguage">Source language identifier (de, en, fr)</param>
        /// <param name="targetLanguage">Target language indentifier (de, en, fr)</param>
        /// <returns>The translated text</returns>
        [HttpGet(Name = nameof(Translate))]
        public string Translate(string input, string sourceLanguage, string targetLanguage)
        {
            return _translatorBusiness.Translate(input, sourceLanguage, targetLanguage);
        }
    }
}