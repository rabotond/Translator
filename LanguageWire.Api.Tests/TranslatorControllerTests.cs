using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using IronPython.Modules;
using LanguageWire.Api.Business;
using LanguageWire.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LanguageWire.Api.Tests
{
    public class TranslatorControllerTests
    {
        [Fact]
        public async Task TranslationController_Should_Return_Ok()
        {
            // Arrange
            var targetLang = "en";
            var inputText = "InputTranslationText";
            var outputText = "OutputTranslationText";
            var mockBusiness = new Mock<ITranslatorBusiness>();
            mockBusiness.Setup(repo => repo.Translate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(outputText);
            var controller = new TranslatorController(mockBusiness.Object);

            // Act
            var result = await controller.Translate(inputText, targetLang);

            // Assert
            result.Should().BeEquivalentTo(outputText);
        }
    }
}