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
        private readonly Dictionary<string, string> inMemorySettings = new Dictionary<string, string>
        {
            {"pythonInstallationPath", @"C:\\Python310\\python.exe"}
        };

        [Theory]
        [InlineData("danke", "thanks")]
        [InlineData("danke nein ja lecker lecker? nein wein wein \n wein nein danke", "thanks no and yummy yummy? no wine wine \n wine no thanks")]
        [InlineData("     danke                ", "thanks")]
        [InlineData("danke     nein danke danke ?????", "thanks no thanks thanks ?????")]
        public async Task TranslationBusiness_English_Should_Success(string inputText, string translation)
        {
            // Arrange
            var targetLang = "en";

            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();


            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = await business.Translate(inputText, targetLang);
            
            // Assert
            translatedText.Should().BeEquivalentTo(translation);
        }

        [Theory]
        [InlineData("danke", "thanks", "en")]
        [InlineData("danke", "merci", "fr")]
        [InlineData("une", "ein", "de")]
        public async Task TranslationBusiness_Check_Languages_Should_Success(string inputText, string translation, string targetLanguage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();


            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = await business.Translate(inputText, targetLanguage);

            // Assert
            translatedText.Should().BeEquivalentTo(translation);
        }

        [Fact]
        public async Task TranslationBusiness_UnsupportedLangue_Return_EmptyString()
        {
            // Arrange
            var targetLang = "unsupportedLanguageType";
            var inputText = "katze";
            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();

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