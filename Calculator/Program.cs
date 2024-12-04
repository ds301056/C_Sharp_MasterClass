using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("Welcome to the Advanced Calculator!");
		Console.WriteLine("Type 'quit' to exit at any time.");

		while (true)
		{
			Console.WriteLine("Please enter a mathematical expression (e.g., 7 + 81 - 12 * 43) or 'quit' to exit:");
			string userInput = Console.ReadLine();

			// Exit if the user types "quit"
			if (userInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
			{
				Console.WriteLine("Thank you for using the calculator. Goodbye!");
				break;
			}

			// Process the input and calculate the result
			try
			{
				double result = EvaluateExpression(userInput);
				Console.WriteLine($"Result: {result}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}
	}

	// Method to evaluate a mathematical expression
	static double EvaluateExpression(string input)
	{
		// Remove all spaces for easier processing
		input = input.Replace(" ", "");

		// Regular expression to split numbers and operators
		string[] numbers = Regex.Split(input, @"[\+\-\*/]");
		string[] operators = Regex.Matches(input, @"[\+\-\*/]").Select(match => match.Value).ToArray();

		// Validate input: ensure the numbers and operators align
		if (numbers.Length - 1 != operators.Length)
		{
			throw new Exception("Invalid expression. Check for missing numbers or operators.");
		}

		// Convert string numbers to doubles
		double[] parsedNumbers = Array.ConvertAll(numbers, num =>
		{
			if (double.TryParse(num, out double parsed))
			{
				return parsed;
			}
			throw new Exception($"Invalid number: {num}");
		});

		// Handle operator precedence: *, /
		List<double> resultNumbers = new List<double> { parsedNumbers[0] };
		List<string> resultOperators = new List<string>();
		for (int i = 0; i < operators.Length; i++)
		{
			if (operators[i] == "*" || operators[i] == "/")
			{
				double lastNumber = resultNumbers[^1];
				double nextNumber = parsedNumbers[i + 1];
				double result = operators[i] == "*" ? lastNumber * nextNumber : lastNumber / nextNumber;

				if (operators[i] == "/" && nextNumber == 0)
				{
					throw new Exception("Division by zero is not allowed.");
				}

				// Replace the last number with the result
				resultNumbers[^1] = result;
			}
			else
			{
				resultNumbers.Add(parsedNumbers[i + 1]);
				resultOperators.Add(operators[i]);
			}
		}

		// Handle remaining operators: +, -
		double finalResult = resultNumbers[0];
		for (int i = 0; i < resultOperators.Count; i++)
		{
			finalResult = resultOperators[i] == "+" ? finalResult + resultNumbers[i + 1] : finalResult - resultNumbers[i + 1];
		}

		return finalResult;
	}
}
