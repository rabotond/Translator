using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageWire.Api.Business;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LanguageWire.Api.Tests
{
    public class TranslatorBusinessTests
    {
        [Fact]
        public async Task TranslationBusiness_Should_Success()
        {
            // Arrange
            var targetLang = "en";
            var inputText = "katze";
            var outputText = "cat";
            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();
            var inMemorySettings = new Dictionary<string, string> {
                {"pythonInstallationPath", @"/Library/Frameworks/Python.framework/Versions/3.10/bin/python3"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = await business.Translate(inputText, targetLang);
            
            // Assert
            translatedText.Should().BeEquivalentTo(outputText);
        }
        
        [Fact]
        public async Task TranslationBusiness_UnsupportedLangue_Return_EmptyString()
        {
            // Arrange
            var targetLang = "unsupportedLanguageType";
            var inputText = "katze";
            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();
            var inMemorySettings = new Dictionary<string, string> {
                {"pythonInstallationPath", @"/Library/Frameworks/Python.framework/Versions/3.10/bin/python3"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = await business.Translate(inputText, targetLang);
            
            // Assert
            translatedText.Should().BeEquivalentTo(string.Empty);
        }
    }
}