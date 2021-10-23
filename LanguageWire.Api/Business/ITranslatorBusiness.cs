using System.Threading.Tasks;

namespace LanguageWire.Api.Business
{
    public interface ITranslatorBusiness
    {
        public Task<string> Translate(string input, string targetLang);
    }
}