using System;
using System.Net.Mime;
using LanguageWire.Api.Business;
using Microsoft.AspNetCore.Mvc;

namespace LanguageWire.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TranslatorController : ControllerBase
    {
        private readonly ITranslatorBusiness _translatorBusiness;
        
        public TranslatorController(ITranslatorBusiness translatorBusiness)
        {
            _translatorBusiness = translatorBusiness ?? throw new ArgumentNullException(nameof(translatorBusiness));
        }

        [HttpGet(Name = nameof(Translate))]
        public string Translate(string input, string sourceLanguage, string targetLanguage)
        {
            return _translatorBusiness.Translate(input, sourceLanguage, targetLanguage);
        }
    }
}