using System;

namespace Companion.Modules.Extensions
{
    /// <summary>
    /// This module provides random number generation functionality.
    /// </summary>
    public class RandomModule : WelcomeModule
    {
        private const string ModuleKeyword = "rand";
        private readonly Random _random;

        public RandomModule() : base(ModuleKeyword)
        {
            _random = new Random();
        }

        public override bool ExecuteCommand(UserRequest userRequest)
        {
            // Check if it's help or exit command first
            if (userRequest.CommandName == ModuleBase._helpCommand || userRequest.CommandName == ModuleBase._exitCommand)
            {
                return base.ExecuteCommand(userRequest);
            }
            
            // For any other command (including empty), generate a random number
            GenerateRandomNumber();
            return true;
        }

        /// <summary>
        /// Generate and display a random number.
        /// </summary>
        private void GenerateRandomNumber()
        {
            var randomNumber = _random.Next();
            Console.WriteLine(randomNumber);
        }
    }
}