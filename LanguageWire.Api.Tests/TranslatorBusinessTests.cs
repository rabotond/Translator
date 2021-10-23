using System.Threading.Tasks;
using FluentAssertions;
using LanguageWire.Api.Business;
using Xunit;

namespace LanguageWire.Api.Tests
{
    public class TranslatorBusinessTests
    {
        [Fact]
        public async Task TranslationController_Should_Work()
        {
            // Arrange
            var targetLang = "en";
            var inputText = "katze";
            var outputText = "cat";

            // Act
            var business = new TranslatorBusiness();
            var translatedText = await business.Translate(inputText, targetLang);
            
            // Assert
            translatedText.Should().BeEquivalentTo(outputText);
        }
    }
}