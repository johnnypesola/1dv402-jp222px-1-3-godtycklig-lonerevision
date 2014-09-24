using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Default namespace for project
namespace _1DV402.S1.L03C
{
    // Use custom extension methods.
    using ExtensionMethods;

    class Program
    {
        static void Main(string[] args)
        {
            int numberOfSalaries;
            int[] salariesArray;

            // Do until user presses Esc key.
            do
            {
                // Clear console, in case its needed.
                Console.Clear();

                // Find out how many salaries the user wants to input.
                numberOfSalaries = ReadInt(Properties.Resources.enterNumberOfSalariesMsg);

                // Ask about the salaries previously mentioned.
                salariesArray = ReadSalaries(numberOfSalaries);

                // View our salaries.
                ViewResult(salariesArray);
            }
            while (IsContinuing()); // Loop until user presses the Esc key.
        }

        /// <summary>
        /// Asks the user if he wants to continue the program.
        /// </summary>
        /// <returns>Bool answer, yes = true, no = false</returns>
        static bool IsContinuing()
        {
            // Set white textcolor and dark green background color
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;

            // Display message for user.
            Console.WriteLine("\n \u25BA {0}\n", Properties.Resources.escapeOrResumeMsg);

            // Reset terminal colors.
            Console.ResetColor();

            // Check if user presses Esc key.
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                // User pressed the Esc key. Return false
                return false;
            }
            else
            {
                // User Presed other key. Return true
                return true;
            }
        }

        /// <summary>
        /// Read input and parse it to an int. Custom exceptions for wrong input declared.
        /// </summary>
        /// <returns>Parsed in from input</returns>
        static int ReadInt(string prompt)
        {
            int parsedInt;
            string promtedValue;
            string displayMessage;
            const int MIN_VALUE = 2;

            // Loop until the user gets the input right.
            while (true)
            {
                // Try to get user input.
                try
                {
                    // Ask the user to enter input for the number of salaries.
                    Console.Write(String.Format("\n {0,-20} {1,-2}", prompt, ":"));

                    // Get user input.
                    promtedValue = Console.ReadLine();

                    // Try to parse user input.
                    if (!int.TryParse(promtedValue, out parsedInt))
                    {
                        // Parse failed. Throw exception with a custom message.
                        throw new FormatException(String.Format("\"{0}\" {1}", promtedValue, Properties.Resources.userInputCouldNotParseMsg));
                    }
                    // Check if number of salaries is less than the lowest accepted value, only apply if we are asking about salaries.
                    else if (parsedInt < MIN_VALUE && prompt == Properties.Resources.enterNumberOfSalariesMsg)
                    {
                        // Throw input out of range exception
                        throw new FormatException(String.Format("{0} ", Properties.Resources.userInputSalariesNumberTooSmallMsg));
                    }
                    // Check if the salary ammount is negative, we can allow that.
                    else if(parsedInt < 0)
                    {
                        throw new FormatException(String.Format("{0} ", Properties.Resources.userInputSalaryTooSmallMsg));
                    }

                    // All seems right. Break loop.
                    break;

                }
                // Catch errors. If there was something wrong with the users input. Display a message for the user.
                catch (Exception exception)
                {
                    // Build a message for user.
                    displayMessage = String.Format("\n {0} {1} ", Properties.Resources.userInputErrorMsg, exception.Message);

                    // Display a error message for user.
                    ViewMessage(displayMessage, ConsoleColor.Red);
                }
            }

            return parsedInt;
        }

        /// <summary>
        /// Get parsed salaries
        /// </summary>
        /// <returns>Array of salaries</returns>
        static int[] ReadSalaries(int count)
        {
            int[] salaryArray = new int[count];

            // Ask user to input every salary.
            for (int i = 0; i < count; i++)
            {
                // Ask user about this salary. Put salary ammount in array.
                salaryArray[i] = ReadInt(String.Format("{0} {1}", Properties.Resources.enterSalaryMsg, i+1));
            }

            return salaryArray;
        }

        /// <summary>
        /// Display console message for user.
        /// </summary>
        static void ViewMessage(string message, ConsoleColor backgroundColor = ConsoleColor.Blue, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            // Set console colors based on arguents.
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            
            // Display message for user.
            Console.WriteLine("\n{0}\n", message);

            // Reset terminal colors.
            Console.ResetColor();
        }
        
        /// <summary>
        /// Print out salary results for the user in console.
        /// </summary>
        static void ViewResult(int[] salaries)
        {
            // Declare variables
            const int COLUMN_COUNT = 3;

            // Clear console, Feels a little better somehow.
            Console.Clear();

            // Write out Median, Average and Dispersion salary results.
            Console.WriteLine("\n--------------------------------");
            Console.WriteLine("{0,-15}: {1,10:c0}", Properties.Resources.textMedianSalary, salaries.Median());
            Console.WriteLine("{0,-15}: {1,10:c0}", Properties.Resources.textAverageSalary, salaries.Average());
            Console.WriteLine("{0,-15}: {1,10:c0}", Properties.Resources.textDispersionSalary, salaries.Dispersion());
            Console.WriteLine("--------------------------------");

            // Write out all salaries in columns of COLUMN_COUNT.
            for(int i = 0; i < salaries.Length; i++)
            {
                // Write out calary
                Console.Write("{0,9}", salaries[i]);

                // Check if its time to change row, do this when COLUMN_COUNT is reached.
                if ((i + 1) % COLUMN_COUNT == 0)
                {
                    Console.WriteLine();
                }
            }

            // Add line before "continue or escape" message
            Console.WriteLine();
        }
    }
}

// Namespace for custom extension methods.
namespace ExtensionMethods
{
    public static class MyExtensions
    {
        /// <summary>
        /// Counts the dispersion in an array
        /// </summary>
        /// <returns>Dispersion value</returns>
        public static int Dispersion(this int[] source)
        {
            // Return dispersion.
            return (source.Max() - source.Min());
        }

        /// <summary>
        /// Counts the median in an array
        /// </summary>
        /// <returns>Median value</returns>
        public static int Median(this int[] source)
        {
            // Declare variables
            int[] sortedArray;
            int returnKey;
            int lowerKey;
            int higherKey;
            int returnValue;

            // Sort array
            sortedArray = source.OrderBy(salary => salary).ToArray();

            // If number of array items are even.
            if(sortedArray.Length % 2 == 0)
            {
                // Figure out which array keys to do math on
                lowerKey = ((sortedArray.Length / 2) - 1);
                higherKey = (sortedArray.Length / 2);

                // Do the median math and assign return value
                returnValue = ((sortedArray[lowerKey] + sortedArray[higherKey]) / 2);
            }
            // Number of array items are odd.
            else
            {
                // Figure out which key we should return,
                returnKey = (sortedArray.Length / 2); /* c# rounds down X.5 values, knows as bankers rounding */

                // Assign return value
                returnValue = sortedArray[returnKey];
            }

            return returnValue;
        }
    }
}
