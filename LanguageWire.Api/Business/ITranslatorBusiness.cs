namespace LanguageWire.Api.Business
{
    public interface ITranslatorBusiness
    {
        public string Translate(string input, string sourceLang, string targetLang);
    }
}