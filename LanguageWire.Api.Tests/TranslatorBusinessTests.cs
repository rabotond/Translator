using System.Threading.Tasks;
using FluentAssertions;
using LanguageWire.Api.Business;
using Xunit;

namespace LanguageWire.Api.Tests
{
    public class TranslatorBusinessTests
    {
        [Fact]
        public async Task Translation_Should_Work()
        {
            var input = "myText";
            var business = new TranslatorBusiness();
            var translatedText = await business.Translate(input);
            translatedText.Should().BeEquivalentTo(input);
        }
    }
}