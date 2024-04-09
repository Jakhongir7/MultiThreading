/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static Semaphore semaphore = new Semaphore(0, 10);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            // feel free to add your code
            // Using Thread class with Join
            Console.WriteLine("Using Thread class with Join:");
            Thread thread = new Thread(() => DecrementAndPrint(10));
            thread.Start();
            thread.Join();

            // Using ThreadPool class with Semaphore
            Console.WriteLine("\nUsing ThreadPool class with Semaphore:");
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DecrementAndPrintUsingThreadPool), 10);
            }

            semaphore.WaitOne();
            Console.WriteLine("\nAll threads completed using ThreadPool.");
            Console.ReadLine();
        }

        static void DecrementAndPrint(object state)
        {
            int number = (int)state;
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {number}");
            number--;

            if (number > 0)
            {
                Thread thread = new Thread(() => DecrementAndPrint(number));
                thread.Start();
                thread.Join();
            }
        }

        static void DecrementAndPrintUsingThreadPool(object state)
        {
            int number = (int)state;
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {number}");
            number--;

            if (number > 0)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DecrementAndPrintUsingThreadPool), number);
            }
            else
            {
                semaphore.Release();
            }
        }
    }
}
