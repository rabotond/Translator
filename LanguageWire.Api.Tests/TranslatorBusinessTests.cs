using System.Collections.Generic;
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
        [InlineData("danke nein ja lecker lecker? nein wein wein \n wein nein danke", "thanks no yes yummy yummy? no wine wine wine no thanks")]
        [InlineData("     danke                ", "thanks")]
        [InlineData("danke     nein danke danke ?????", "thanks no thanks thanks ?????")]
        public void TranslationBusiness_English_Should_Success(string inputText, string translation)
        {
            // Arrange
            var srcLang = "de";
            var targetLang = "en";

            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();


            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = business.Translate(inputText, srcLang, targetLang);

            // Assert
            translatedText.Should().BeEquivalentTo(translation);
        }

        [Theory]
        [InlineData("danke", "thanks", "de", "en")]
        [InlineData("danke", "merci", "de", "fr")]
        [InlineData("une", "ein", "fr", "de")]
        public void TranslationBusiness_Check_Languages_Should_Success(string inputText, string translation, string sourceLanguage, string targetLanguage)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();


            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = business.Translate(inputText, sourceLanguage, targetLanguage);

            // Assert
            translatedText.Should().BeEquivalentTo(translation);
        }

        [Theory]
        [InlineData("katze", "unsupportedLanguageType", "en")]
        [InlineData("katze", "unsupportedLanguageType", "unsupportedLanguageType2")]
        [InlineData("cat", "en", "unsupportedLanguageType")]
        public void TranslationBusiness_UnsupportedLangue_Return_EmptyString(string inputText, string srcLang, string destLang)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<TranslatorBusiness>>();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act
            var business = new TranslatorBusiness(mockLogger.Object, configuration);
            var translatedText = business.Translate(inputText, srcLang, destLang);

            // Assert
            translatedText.Should().BeEquivalentTo(string.Empty);
        }
    }
}