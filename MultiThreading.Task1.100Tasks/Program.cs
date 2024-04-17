/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            HundredTasks();

            Console.ReadLine();
        }

        static void HundredTasks()
        {
            // Create an array of 100 Tasks
            Task[] tasks = new Task[TaskAmount];

            // Populate the array with Tasks
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => TaskMethod(i));
            }

            // Wait for all tasks to complete
            Task.WaitAll(tasks);

            Console.WriteLine("All tasks completed.");
        }

        static void TaskMethod(int taskNumber)
        {
            // Iterate from 1 to 1000 and print the string
            for (int i = 1; i <= MaxIterationsCount; i++)
            {
                Console.WriteLine($"Task #{taskNumber} - {i}");
            }
        }
    }
}
