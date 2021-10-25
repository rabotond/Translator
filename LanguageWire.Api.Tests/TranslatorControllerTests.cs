using FluentAssertions;
using LanguageWire.Api.Business;
using LanguageWire.Api.Controllers;
using Moq;
using Xunit;

namespace LanguageWire.Api.Tests
{
    public class TranslatorControllerTests
    {
        [Fact]
        public void TranslationController_Should_Return_Ok()
        {
            // Arrange
            var sourceLang = "de";
            var targetLang = "en";
            var inputText = "InputTranslationText";
            var outputText = "OutputTranslationText";
            var mockBusiness = new Mock<ITranslatorBusiness>();
            mockBusiness.Setup(repo => repo.Translate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(outputText);
            var controller = new TranslatorController(mockBusiness.Object);

            // Act
            var result = controller.Translate(inputText, sourceLang, targetLang);

            // Assert
            result.Should().BeEquivalentTo(outputText);
        }
    }
}