/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // First Task: Creates an array of 10 random integers
            var firstTask = Task.Run(() =>
            {
                Random random = new Random();
                int[] randomArray = Enumerable.Range(1, 10).Select(_ => random.Next(1, 101)).ToArray();
                Console.WriteLine("Generated Array:");
                PrintArray(randomArray);
                return randomArray;
            });

            // Second Task: Multiplies the array with another random integer
            var secondTask = firstTask.ContinueWith(previousTask =>
            {
                int[] array = previousTask.Result;
                Random random = new Random();
                int multiplier = random.Next(1, 11);
                Console.WriteLine($"Multiplier: {multiplier}");
                int[] multipliedArray = array.Select(x => x * multiplier).ToArray();
                Console.WriteLine("Multiplied Array:");
                PrintArray(multipliedArray);
                return multipliedArray;
            });

            // Third Task: Sorts the array by ascending order
            var thirdTask = secondTask.ContinueWith(previousTask =>
            {
                int[] array = previousTask.Result;
                Array.Sort(array);
                Console.WriteLine("Sorted Array:");
                PrintArray(array);
                return array;
            });

            // Fourth Task: Calculates the average value
            var fourthTask = thirdTask.ContinueWith(previousTask =>
            {
                int[] array = previousTask.Result;
                double average = array.Average();
                Console.WriteLine($"Average Value: {average}");
            });

            await fourthTask; // Wait for the completion of the fourth task

            Console.ReadLine();
        }

        static void PrintArray(int[] array)
        {
            foreach (var item in array)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }
    }
}
