using System;
using System.Threading.Tasks;
using LanguageWire.Api.Business;
using Microsoft.AspNetCore.Mvc;

namespace LanguageWire.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslatorController : ControllerBase
    {
        private readonly ITranslatorBusiness _translatorBusiness;
        
        public TranslatorController(ITranslatorBusiness translatorBusiness)
        {
            _translatorBusiness = translatorBusiness ?? throw new ArgumentNullException(nameof(translatorBusiness));
        }
        
        [HttpGet]
        public async Task<string> Translate(string input)
        {
            var result = await _translatorBusiness.Translate(input);
            return result;
        }
    }
}