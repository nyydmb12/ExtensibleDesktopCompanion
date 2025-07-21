using Companion.Modules.Extensions;
using Companion.Modules;
using System.IO;
using Xunit;

namespace Companion.Modules.Tests
{
    public class RandomModule_Tests
    {
        [Fact]
        public void ShouldCreateRandomModuleWithCorrectKeyword()
        {
            // Arrange & Act
            var randomModule = new RandomModule();

            // Assert
            Assert.Equal("rand", randomModule.GetModuleKeyword());
        }

        [Fact]
        public void ShouldGenerateRandomNumberWhenExecutingUnknownCommand()
        {
            // Arrange
            var randomModule = new RandomModule();
            var userRequest = new UserRequest
            {
                ModuleName = "rand",
                CommandName = "unknowncommand",
                CommandParameters = new string[0]
            };
            
            // Capture console output
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                
                try
                {
                    // Act
                    var result = randomModule.ExecuteCommand(userRequest);
                    
                    // Assert
                    Assert.True(result);
                    var output = sw.ToString().Trim();
                    Assert.True(int.TryParse(output, out int _), $"Output should be a valid integer, but was: '{output}'");
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void ShouldExecuteHelpCommand()
        {
            // Arrange
            var randomModule = new RandomModule();
            var userRequest = new UserRequest
            {
                ModuleName = "rand",
                CommandName = "help",
                CommandParameters = new string[0]
            };

            // Capture console output to prevent disposal issues
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                
                try
                {
                    // Act
                    var result = randomModule.ExecuteCommand(userRequest);

                    // Assert
                    Assert.True(result);
                    var output = sw.ToString();
                    Assert.Contains("These are the avaialable commands:", output);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        [Fact]
        public void ShouldGenerateDifferentNumbers()
        {
            // Arrange
            var randomModule = new RandomModule();
            var numbers = new List<int>();

            // Act - generate multiple random numbers
            for (int i = 0; i < 10; i++)
            {
                var userRequest = new UserRequest
                {
                    ModuleName = "rand",
                    CommandName = "generate",
                    CommandParameters = new string[0]
                };

                using (var sw = new StringWriter())
                {
                    var originalOut = Console.Out;
                    Console.SetOut(sw);
                    
                    try
                    {
                        randomModule.ExecuteCommand(userRequest);
                        var output = sw.ToString().Trim();
                        if (int.TryParse(output, out int number))
                        {
                            numbers.Add(number);
                        }
                    }
                    finally
                    {
                        Console.SetOut(originalOut);
                    }
                }
            }

            // Assert - at least some numbers should be different (very unlikely all 10 would be the same)
            var uniqueNumbers = numbers.Distinct().Count();
            Assert.True(uniqueNumbers > 1, "Should generate different random numbers");
            Assert.Equal(10, numbers.Count);
        }
    }
}