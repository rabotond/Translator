using System.Threading.Tasks;
using FluentAssertions;
using LanguageWire.Api.Business;
using Xunit;

namespace LanguageWire.Api.Tests
{
    public class TranslatorControllerTests
    {
        [Fact]
        public async Task Translate_Should_Success()
        {
            var input = "myText";
            var business = new TranslatorBusiness();
            var translatedText = await business.Translate(input);
            translatedText.Should().BeEquivalentTo(input);
        }
    }
}