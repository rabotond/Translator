using System;
using System.Net.Mime;
using System.Threading.Tasks;
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
        public async Task<string> Translate(string input, string targetLang)
        {
            return await _translatorBusiness.Translate(input, targetLang);
        }
    }
}