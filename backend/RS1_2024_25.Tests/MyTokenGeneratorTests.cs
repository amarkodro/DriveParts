using RS1_2024_25.API.Helper;
using Xunit;

namespace RS1_2024_25.Tests
{
    public class MyTokenGeneratorTests
    {
        [Fact]
        public void Generate_ShouldReturnTokenOfRequestedLength()
        {
            // Arrange
            var generator = new MyTokenGenerator();
            int size = 20;

            // Act
            var token = generator.Generate(size);

            // Assert
            Assert.NotNull(token);
            Assert.Equal(size, token.Length);
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GenerateName_ShouldReturnTokenStartingWithS()
        {
            // Arrange
            var generator = new MyTokenGenerator();
            int size = 10;

            // Act
            var token = generator.GenerateName(size);

            // Assert
            Assert.NotNull(token);
            Assert.StartsWith("S", token);
            Assert.Equal(size + 1, token.Length);
        }
    }
}